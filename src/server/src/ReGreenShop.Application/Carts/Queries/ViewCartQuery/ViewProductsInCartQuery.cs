using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Carts.Queries.ViewProductsInCartQuery.Models;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Services;

namespace ReGreenShop.Application.Carts.Queries.ViewProductsInCartQuery;
public record ViewProductsInCartQuery : IRequest<CartModel>
{
    public class ViewProductsInCartQueryHandler : IRequestHandler<ViewProductsInCartQuery, CartModel>
    {
        private readonly ICart cartService;
        private readonly IData data;

        public ViewProductsInCartQueryHandler(ICart cartService, IData data)
        {
            this.cartService = cartService;
            this.data = data;
        }

        public async Task<CartModel> Handle(ViewProductsInCartQuery request, CancellationToken cancellationToken)
        {
            var cartId = await this.cartService.GetCartIdAsync();
   
            var cartItemDtos = await this.data.CartItems
                .Where(x => x.CartId == cartId)
                .Select(x => new
                {
                    x.ProductId,
                    x.BaseCategoryId,
                    x.Quantity,
                })
                .ToListAsync();

            var cartItems = cartItemDtos
                .GroupBy(x => x.BaseCategoryId)
                .Select(group => new ProductsByCategory
                {
                    Id = group.Key,
                    CategoryName = this.data.Categories
                                        .Where(c => c.Id == group.Key)
                                        .Select(c => c.NameInEnglish)
                                        .FirstOrDefault() ?? "Unknown",
                    Products = this.data.Products
                                .Where(p => group.Select(ci => ci.ProductId).Contains(p.Id))
                                .To<ProductInCartModel>()
                                .ToList()
                                .Select(dto =>
                                {
                                    var quantity = group.First(x => x.ProductId == dto.Id).Quantity;
                                    dto.QuantityInCart = quantity;
                                    return dto;
                                }).ToList()
      
                })
                .ToList();


            foreach (var productList in cartItems.Select(x => x.Products))
            {
                foreach (var prod in productList)
                {
                    if (prod.HasTwoForOneDiscount)
                    {
                        prod.DiscountPrice = PriceCalculator.CalculateTwoForOnePriceSinglePrice(prod.Price);
                        prod.TotalPriceProduct = PriceCalculator.CalculateTwoForOnePrice(prod.Price, prod.QuantityInCart);
                        prod.LabelTwoForOne = "TwoForOne";
                    }
                    else if (prod.HasPromoDiscount && !prod.HasTwoForOneDiscount && prod.DiscountPercentage.HasValue)
                    {
                        prod.DiscountPrice = PriceCalculator.CalculateDiscountedPrice(prod.Price, prod.DiscountPercentage.Value);
                        prod.TotalPriceProduct = PriceCalculator.CalculateTotalPrice(prod.DiscountPrice.Value, prod.QuantityInCart);
                    }
                    else
                    {
                        prod.TotalPriceProduct = PriceCalculator.CalculateTotalPrice(prod.Price, prod.QuantityInCart);
                    }
                }
            }

            decimal? deliveryCost;
            string deliveryMessage = string.Empty;

            var totalPriceProducts = cartItems.SelectMany(x => x.Products).Sum(x => x.TotalPriceProduct);
            var deliveryTiers =  await this.data.DeliveryPrices
                        .OrderBy(x => x.MinPriceOrder)
                        .ToListAsync();

            var minDeliveryTier = deliveryTiers.First();
            var freeDeliveryTier = deliveryTiers.Last();

            var deliveryPriceTier = deliveryTiers
                        .FirstOrDefault(x =>
                         totalPriceProducts >= x.MinPriceOrder && totalPriceProducts <= x.MaxPriceOrder);

            if (deliveryPriceTier == null)
            {
                deliveryCost = null;
                deliveryMessage = $"Minimum order value for delivery is {minDeliveryTier.MinPriceOrder}";
            }
            else
            {
                deliveryCost = deliveryPriceTier.Price;
                if (deliveryPriceTier.Price > 0m)
                {
                    deliveryMessage = $"Add items worth {freeDeliveryTier.MinPriceOrder - totalPriceProducts : 0.00}lv more to get a free delivery";
                }
                else
                {
                    deliveryMessage = "You get free delivery!";
                }
            }

            var cartModel = new CartModel
            {
                ProductsByCategories = cartItems,
                TotalPrice = totalPriceProducts,
                DeliveryMessage = deliveryMessage,
                DeliveryPriceProducts = deliveryCost,
            };

            return cartModel;
        }
    }
}

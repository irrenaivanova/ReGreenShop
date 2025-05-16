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
        private readonly IDelivery deliveryService;

        public ViewProductsInCartQueryHandler(ICart cartService, IData data, IDelivery deliveryService)
        {
            this.cartService = cartService;
            this.data = data;
            this.deliveryService = deliveryService;
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

            // TODO : Calvculating the price of the products in the cart in a service
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
            var totalPriceProducts = cartItems.SelectMany(x => x.Products).Sum(x => x.TotalPriceProduct);


            (decimal? deliveryCost, string deliveryMessage) =  this.deliveryService.CalculateTheDeliveryPrice(totalPriceProducts);

            var cartModel = new CartModel
            {
                ProductsByCategories = cartItems,
                TotalPrice = totalPriceProducts,
                DeliveryMessage = deliveryMessage,
                DeliveryPriceProducts = deliveryCost != null ? Math.Round(deliveryCost.Value,2) : default!
            };

            return cartModel;
        }
    }
}

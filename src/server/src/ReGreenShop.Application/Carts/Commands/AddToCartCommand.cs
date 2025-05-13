using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Carts.Commands;
public record AddToCartCommand(int id) : IRequest<Unit>
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Unit>
    {
        private readonly IData data;
        private readonly ICart cartService;

        public AddToCartCommandHandler(IData data, ICart cartService)
        {
            this.data = data;
            this.cartService = cartService;
        }

        public async Task<Unit> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var product = this.data.Products.Include(x => x.ProductCategories).ThenInclude(x => x.Category).SingleOrDefault(x => x.Id == request.id);
            if (product == null)
            {
                throw new NotFoundException("Product", request.id);
            }
            var cartId = await this.cartService.GetCartIdAsync();
            var cartItem = this.data.CartItems.SingleOrDefault(x => x.CartId == cartId && x.ProductId == request.id);

            int productStock = product.Stock;
            if (cartItem == null)
            {
                if (productStock == 0)
                {
                    throw new InsufficientQuantityException(product.Name);
                }
                cartItem = new CartItem()
                {
                    CartId = cartId!,
                    ProductId = request.id,
                    Quantity = 1,
                    BaseCategoryId = product.ProductCategories.FirstOrDefault(x => x.Category.ParentCategory == null)!.CategoryId
                };

                this.data.CartItems.Add(cartItem);
            }
            else
            {
                if (productStock < cartItem.Quantity + 1)
                {
                    throw new InsufficientQuantityException(product.Name);
                }
                cartItem.Quantity += 1;
            }
            await this.data.SaveChangesAsync();
            return Unit.Value;
        }
    }
}

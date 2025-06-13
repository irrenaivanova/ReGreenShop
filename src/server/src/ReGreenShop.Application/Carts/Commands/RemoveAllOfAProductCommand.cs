using MediatR;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.Carts.Commands;
public record RemoveFromCartCommand(int id) : IRequest<Unit>
{
    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, Unit>
    {
        private readonly IData data;
        private readonly ICart cartService;

        public RemoveFromCartCommandHandler(IData data, ICart cartService)
        {
            this.data = data;
            this.cartService = cartService;
        }

        public async Task<Unit> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var product = this.data.Products.SingleOrDefault(x => x.Id == request.id);
            if (product == null)
            {
                throw new NotFoundException("Product", request.id);
            }
            var cartId = await this.cartService.GetCartIdAsync();
            var cartItem = this.data.CartItems.SingleOrDefault(x => x.CartId == cartId && x.ProductId == request.id);
            if (cartItem == null)
            {
                throw new NotFoundException("CartItem", "null");
            }

            if (cartItem != null)
            {
                cartItem.Quantity -= 1;
                if (cartItem.Quantity == 0)
                {
                    this.data.CartItems.Remove(cartItem);
                }
            }
            await this.data.SaveChangesAsync();
            return Unit.Value;
        }
    }
}

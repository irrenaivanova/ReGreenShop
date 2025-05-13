using MediatR;
using ReGreenShop.Application.Carts.Queries.ViewProductsInCartQuery.Models;
using ReGreenShop.Application.Common.Interfaces;

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

        public Task<CartModel> Handle(ViewProductsInCartQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

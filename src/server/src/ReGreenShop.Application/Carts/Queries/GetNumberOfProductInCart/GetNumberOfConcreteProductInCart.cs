using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.Carts.Queries.GetNumberOfProductInCart;
public record GetNumberOfConcreteProductInCart(int id) : IRequest<int>
{
    public class GetNumberOfConcreteProductInCartHandler : IRequestHandler<GetNumberOfConcreteProductInCart, int>
    {
        private readonly ICart cartService;

        public GetNumberOfConcreteProductInCartHandler(ICart cartService)
        {
            this.cartService = cartService;
        }

        public Task<int> Handle(GetNumberOfConcreteProductInCart request, CancellationToken cancellationToken)
        {
            return this.cartService.GetCountOfConcreteProductInCartAsync(request.id);
        }
    }
}

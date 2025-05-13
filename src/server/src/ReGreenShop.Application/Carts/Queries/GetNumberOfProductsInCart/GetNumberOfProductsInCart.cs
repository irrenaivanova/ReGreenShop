using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.Carts.Queries.GetNumberOfProductsInCart;
public record GetNumberOfProductsInCart : IRequest<int>
{
    public class GetNumberOfProductsInCartHandler : IRequestHandler<GetNumberOfProductsInCart, int>
    {
        private readonly ICart cartService;

        public GetNumberOfProductsInCartHandler(ICart cartService)
        {
            this.cartService = cartService;
        }

        public async Task<int> Handle(GetNumberOfProductsInCart request, CancellationToken cancellationToken)
        {
            return await this.cartService.GetCountProductsInCartAsync();
        }
    }
}

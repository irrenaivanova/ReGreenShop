using MediatR;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.Carts.Commands;
public record CleanCartCommand : IRequest<Unit>
{
    public class CleanCartCommandHandler : IRequestHandler<CleanCartCommand, Unit>
    {
        private readonly ICart cartService;

        public CleanCartCommandHandler(ICart cartService)
        {
            this.cartService = cartService;
        }

        public async Task<Unit> Handle(CleanCartCommand request, CancellationToken cancellationToken)
        {
            await this.cartService.ClearCartAsync();
            return Unit.Value;
        }
    }
}

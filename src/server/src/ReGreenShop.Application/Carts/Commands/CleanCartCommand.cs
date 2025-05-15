using MediatR;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.Carts.Commands;
public record CleanCartCommand : IRequest<Unit>
{
    public class CleanCartCommandHandler : IRequestHandler<CleanCartCommand, Unit>
    {
        private readonly ICart cartService;
        private readonly IData data;

        public CleanCartCommandHandler(ICart cartService, IData data)
        {
            this.cartService = cartService;
            this.data = data;
        }

        public async Task<Unit> Handle(CleanCartCommand request, CancellationToken cancellationToken)
        {
            await this.cartService.ClearCartAsync();
            await this.data.SaveChangesAsync();
            return Unit.Value;
        }
    }
}

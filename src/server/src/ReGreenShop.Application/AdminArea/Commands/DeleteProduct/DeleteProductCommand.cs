using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.AdminArea.Commands.DeleteProduct;
public record DeleteProductCommand(int id) : IRequest<Unit>
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IData data;

        public DeleteProductCommandHandler(IData data)
        {
            this.data = data;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productToDelete = await this.data.Products.FirstOrDefaultAsync(x => x.Id == request.id);
            if (productToDelete == null)
            {
                throw new NotFoundException("Product");
            }

            productToDelete.IsDeleted = true;
            await this.data.SaveChangesAsync();
            return Unit.Value;
        }
    }
}

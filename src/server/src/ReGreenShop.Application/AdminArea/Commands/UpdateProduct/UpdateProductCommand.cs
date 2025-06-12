using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.AdminArea.Commands.UpdateProduct;
public record UpdateProductCommand(UpdateProductModel model) : IRequest<Unit>
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IData data;

        public UpdateProductCommandHandler(IData data)
        {
            this.data = data;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productToUpdate = await this.data.Products.FirstOrDefaultAsync(x => x.Id == request.model.Id);
            if (productToUpdate == null)
            {
                throw new NotFoundException("Product");
            }
            productToUpdate.Price = request.model.Price;
            productToUpdate.Stock = request.model.Stock;
            await this.data.SaveChangesAsync();
            return Unit.Value;
        }
    }
}

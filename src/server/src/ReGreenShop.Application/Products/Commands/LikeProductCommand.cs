using MediatR;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Products.Commands;
public record LikeProductCommand(int id) : IRequest<Unit>
{
    public class LikeProductHandler : IRequestHandler<LikeProductCommand, Unit>
    {
        private readonly IData data;
        private readonly ICurrentUser userService;

        public LikeProductHandler(IData data, ICurrentUser userService)
        {
            this.data = data;
            this.userService = userService;
        }

        public async Task<Unit> Handle(LikeProductCommand request, CancellationToken cancellationToken)
        {
            var userId = this.userService.UserId;
            if (userId == null)
            {
                throw new BusinessRulesException("You must be logged in to use this feature");
            }
            var product = this.data.Products.FirstOrDefault(x => x.Id == request.id);
            if (product == null)
            {
                throw new NotFoundException("Product", request.id);
            }

            var userLike = this.data.UserLikeProducts.Where(x => x.ProductId == request.id && x.UserId == userId).SingleOrDefault();

            if (userLike == null)
            {
                product.UserLikes.Add(new UserLikeProduct()
                {
                    ProductId = request.id,
                    UserId = userId,
                });
            }
            else
            {
                this.data.UserLikeProducts.Remove(userLike);
            }
            await this.data.SaveChangesAsync();
            return Unit.Value;
        }
    }
}

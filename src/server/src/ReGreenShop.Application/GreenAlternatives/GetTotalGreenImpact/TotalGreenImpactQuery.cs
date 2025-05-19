using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.GreenAlternatives.GetTotalGreenImpact;
public class TotalGreenImpactQuery : IRequest<IEnumerable<TotalGreenImpactModel>>
{
    public class TotalGreenImpactQueryHandler : IRequestHandler<TotalGreenImpactQuery, IEnumerable<TotalGreenImpactModel>>
    {
        private readonly IData data;
        private readonly ICurrentUser currentUser;

        public TotalGreenImpactQueryHandler(IData data, ICurrentUser currentUser)
        {
            this.data = data;
            this.currentUser = currentUser;
        }

        public async Task<IEnumerable<TotalGreenImpactModel>> Handle(TotalGreenImpactQuery request, CancellationToken cancellationToken)
        {
            var userId = this.currentUser.UserId;
            if (userId == null)
            {
                throw new NotFoundException("User");
            }
            var totalGreenImpact = await this.data.Orders.Where(x => x.UserId == userId)
                .Include(x => x.OrderGreenAlternativeDetails)
                .ThenInclude(x => x.GreenAlternative)
                .SelectMany(x => x.OrderGreenAlternativeDetails)
                .GroupBy(x => x.GreenAlternativeId)
                .Select(x => new TotalGreenImpactModel()
                {
                    Name = this.data.GreenAlternatives.FirstOrDefault(y => y.Id == x.Key)!.Name,
                    Quantity = x.Sum(x => x.Quantity)
                }).ToListAsync();

            return totalGreenImpact;
        }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Orders.Queries.Models;

namespace ReGreenShop.Application.Orders.Queries.GetMyOrdersQuery;
public class GetMyOrdersQuery : IRequest<IEnumerable<GetOrderModel>>
{
    public class GetMyOrderQueryHandler : IRequestHandler<GetMyOrdersQuery, IEnumerable<GetOrderModel>>
    {
        private readonly ICurrentUser currentUser;
        private readonly IData data;

        public GetMyOrderQueryHandler(ICurrentUser currentUser, IData data)
        {
            this.currentUser = currentUser;
            this.data = data;
        }

        public async Task<IEnumerable<GetOrderModel>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
        {
            var userId = this.currentUser.UserId;
            var orderDetails = await this.data.Orders.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .To<GetOrderModel>()
                .ToListAsync();

            return orderDetails;
        }
    }
}

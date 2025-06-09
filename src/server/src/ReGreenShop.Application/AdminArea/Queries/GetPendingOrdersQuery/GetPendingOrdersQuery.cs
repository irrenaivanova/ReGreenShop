using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Orders.Queries.Models;

namespace ReGreenShop.Application.Orders.Queries.GetPendingOrdersQuery;
public class GetPendingOrdersQuery : IRequest<IEnumerable<GetOrderModel>>
{
    public class GetPendingOrdersQueryHandler : IRequestHandler<GetPendingOrdersQuery, IEnumerable<GetOrderModel>>
    {
        private readonly IData data;

        public GetPendingOrdersQueryHandler(IData data)
        {
            this.data = data;
        }

        public async Task<IEnumerable<GetOrderModel>> Handle(GetPendingOrdersQuery request, CancellationToken cancellationToken)
        {
            var orderDetails = await this.data.Orders.Where(x => x.Status == Domain.Entities.Enum.OrderStatus.Pending)
                .To<GetOrderModel>()
                .ToListAsync();

            return orderDetails;
        }
    }
}

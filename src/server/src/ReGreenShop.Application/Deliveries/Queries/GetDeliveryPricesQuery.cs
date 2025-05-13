using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;

namespace ReGreenShop.Application.Deliveries.Queries;
public record GetDeliveryPricesQuery : IRequest<IEnumerable<DeliveryPriceModel>>
{
    public class GetDeliveryPricesHandler : IRequestHandler<GetDeliveryPricesQuery, IEnumerable<DeliveryPriceModel>>
    {
        private readonly IData data;

        public GetDeliveryPricesHandler(IData data)
        {
            this.data = data;
        }

        public async Task<IEnumerable<DeliveryPriceModel>> Handle(GetDeliveryPricesQuery request, CancellationToken cancellationToken)
        {
            return await this.data.DeliveryPrices
                 .OrderBy(x => x.MinPriceOrder)
                 .To<DeliveryPriceModel>()
                 .ToListAsync();
        }
    }
}

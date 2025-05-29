using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;

namespace ReGreenShop.Application.Payments.Queries;
public record GetAllPaymentMethodQuery : IRequest<IEnumerable<PaymentModel>>
{
    public class GetAllPaymentMethodQueryHandler : IRequestHandler<GetAllPaymentMethodQuery, IEnumerable<PaymentModel>>
    {
        private readonly IData data;

        public GetAllPaymentMethodQueryHandler(IData data)
        {
            this.data = data;
        }

        public async Task<IEnumerable<PaymentModel>> Handle(GetAllPaymentMethodQuery request, CancellationToken cancellationToken)
        {
            return await this.data.PaymentMethods.To<PaymentModel>().ToListAsync();
        }
    }
}

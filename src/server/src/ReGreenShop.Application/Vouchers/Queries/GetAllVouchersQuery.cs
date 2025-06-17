using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;

namespace ReGreenShop.Application.Vouchers.Queries;
public record GetAllVouchersQuery : IRequest<IEnumerable<VoucherModel>>
{
    public class GetAllVouchersQueryHandler : IRequestHandler<GetAllVouchersQuery, IEnumerable<VoucherModel>>
    {
        private readonly IData data;

        public GetAllVouchersQueryHandler(IData data)
        {
            this.data = data;
        }

        public async Task<IEnumerable<VoucherModel>> Handle(GetAllVouchersQuery request, CancellationToken cancellationToken)
        {
            return await this.data.DiscountVouchers.OrderBy(x => x.GreenPoints).To<VoucherModel>().ToListAsync();
        }
    }
}

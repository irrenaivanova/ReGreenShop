using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;

namespace ReGreenShop.Application.GreenAlternatives;
public class GetAllGreenAlternativesQuery : IRequest<IEnumerable<GreenAlternativesModel>>
{
    public class GetAllGreenAlternativesQueryHandler : IRequestHandler<GetAllGreenAlternativesQuery, IEnumerable<GreenAlternativesModel>>
{
        private readonly IData data;

        public GetAllGreenAlternativesQueryHandler(IData data)
        {
            this.data = data;
        }

        public async Task<IEnumerable<GreenAlternativesModel>> Handle(GetAllGreenAlternativesQuery request, CancellationToken cancellationToken)
        {
            return await this.data.GreenAlternatives.To<GreenAlternativesModel>().ToListAsync();
        }
    }
}

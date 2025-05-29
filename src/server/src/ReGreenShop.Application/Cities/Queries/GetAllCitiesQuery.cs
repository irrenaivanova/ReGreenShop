using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;

namespace ReGreenShop.Application.Cities.Queries;
public record GetAllCitiesQuery : IRequest<IEnumerable<CityModel>>
{
    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, IEnumerable<CityModel>>
    {
        private readonly IData data;

        public GetAllCitiesQueryHandler(IData data)
        {
            this.data = data;
        }

        public async Task<IEnumerable<CityModel>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            return await this.data.Cities.To<CityModel>().ToListAsync();
        }
    }
}

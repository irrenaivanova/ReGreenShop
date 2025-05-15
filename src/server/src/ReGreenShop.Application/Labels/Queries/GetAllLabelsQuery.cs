using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;

namespace ReGreenShop.Application.Labels.Queries;
public record GetAllLabelsQuery : IRequest<IEnumerable<LabelModel>>
{
    public class GetAllLabelsHandler : IRequestHandler<GetAllLabelsQuery, IEnumerable<LabelModel>>
    {
        private readonly IData data;

        public GetAllLabelsHandler(IData data)
        {
            this.data = data;
        }

        public async Task<IEnumerable<LabelModel>> Handle(GetAllLabelsQuery request, CancellationToken cancellationToken)
        {
            var labels = await this.data.Labels.To<LabelModel>().ToListAsync();
            return labels;
        }
    }
}

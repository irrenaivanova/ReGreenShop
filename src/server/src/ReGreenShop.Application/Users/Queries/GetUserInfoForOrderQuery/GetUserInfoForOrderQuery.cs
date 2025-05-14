using MediatR;

namespace ReGreenShop.Application.Users.Queries.GetUserInfoForOrderQuery;
public record GetUserInfoForOrderQuery : IRequest<GetUserInfoForOrderModel>
{
    public class GetUserInfoForOrderQueryHandler : IRequestHandler<GetUserInfoForOrderQuery, GetUserInfoForOrderModel>
    {
        public Task<GetUserInfoForOrderModel> Handle(GetUserInfoForOrderQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

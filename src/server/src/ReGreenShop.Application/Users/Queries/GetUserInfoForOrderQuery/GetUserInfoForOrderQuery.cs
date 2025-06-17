using MediatR;
using ReGreenShop.Application.Common.Identity;

namespace ReGreenShop.Application.Users.Queries.GetUserInfoForOrderQuery;
public record GetUserInfoForOrderQuery : IRequest<UserInfoForOrderModel>
{
    public class GetUserInfoForOrderQueryHandler : IRequestHandler<GetUserInfoForOrderQuery, UserInfoForOrderModel>
    {
        private readonly IIdentity identityService;

        public GetUserInfoForOrderQueryHandler(IIdentity identityService)
        {
            this.identityService = identityService;
        }

        public async Task<UserInfoForOrderModel> Handle(GetUserInfoForOrderQuery request, CancellationToken cancellationToken)
        {
            var userInfo = await this.identityService.GetUserWithAdditionalInfo();
            return userInfo;
        }
    }
}

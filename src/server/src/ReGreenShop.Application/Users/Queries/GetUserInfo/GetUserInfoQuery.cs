
using MediatR;
using ReGreenShop.Application.Common.Identity;

namespace ReGreenShop.Application.Users.Queries.GetUserInfo;
public record GetUserInfoQuery : IRequest<GetUserInfoModel>
{
    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, GetUserInfoModel>
    {
        private readonly IIdentity userService;

        public GetUserInfoQueryHandler(IIdentity userService)
        {
            this.userService = userService;
        }

        public async Task<GetUserInfoModel> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            return await this.userService.GetUserInfoAsync();
        }
    }
}

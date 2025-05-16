using ReGreenShop.Application.Common.Services;
using ReGreenShop.Application.Orders.Commands.MakeAnOrder;
using ReGreenShop.Application.Users.Queries.GetUserInfo;
using ReGreenShop.Application.Users.Queries.GetUserInfoForOrderQuery;

namespace ReGreenShop.Application.Common.Identity;
public interface IIdentity : IScopedService
{
    Task<AuthResponse> LoginUserAsync(string username, string password);

    Task<AuthResponse> RegisterUserAsync(string username, string password);

    Task<string?> GetUserName(string userId);

    Task<UserInfoForOrderModel> GetUserWithAdditionalInfo();

    Task ChangeUserInfoAsync(ChangeUserModel model);

    Task<GetUserInfoModel> GetUserInfoAsync();
}

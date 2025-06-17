using MediatR;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.Common.Identity.Login;
public record HandleGoogleLoginCommand(string ReturnUrl) : IRequest<AuthResponse>
{
    public class HandleGoogleLoginCommandHandler : IRequestHandler<HandleGoogleLoginCommand, AuthResponse>
    {
        private readonly IExternalAuth externalAuthService;
        private readonly IIdentity identityService;
        private readonly ICart cartService;

        public HandleGoogleLoginCommandHandler(IExternalAuth externalAuthService, IIdentity identityService, ICart cartService)
        {
            this.externalAuthService = externalAuthService;
            this.identityService = identityService;
            this.cartService = cartService;
        }

        public async Task<AuthResponse> Handle(HandleGoogleLoginCommand request, CancellationToken cancellationToken)
        {
            var loginInfo = await this.externalAuthService.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                throw new Exception("External login info not found");
            }

            var response = await this.identityService.LoginOrRegisterExternalAsync(loginInfo);
            var authResponse = response.Item1;
            var isRegister = response.Item2;
            if (isRegister)
            {
                await this.cartService.CreateCartAsync(authResponse.UserId);
            }

            await this.cartService.MergeCartIfAnyAsync(authResponse.UserId);
            return authResponse;
        }
    }
}



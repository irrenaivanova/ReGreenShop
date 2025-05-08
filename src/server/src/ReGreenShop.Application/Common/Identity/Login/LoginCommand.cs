using MediatR;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.Common.Identity.Login;
public class LoginCommand : IRequest<AuthResponse>
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IIdentity identityService;
        private readonly ICart cartService;

        public LoginCommandHandler(IIdentity identityService, ICart cartService)
        {
            this.identityService = identityService;
            this.cartService = cartService;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var authResponse = await this.identityService.LoginUserAsync(request.UserName, request.Password);
            await this.cartService.MergeCartIfAnyAsync(authResponse.UserId);
            return authResponse;
        }
    }
}

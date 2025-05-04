using MediatR;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.Common.Identity.Register;
public class RegisterCommand : IRequest<AuthResponse>
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly IIdentity identityService;
        private readonly ICart cartService;

        public RegisterCommandHandler(IIdentity identityService, ICart cartService)
        {
            this.identityService = identityService;
            this.cartService = cartService;
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var authResponse = await this.identityService.RegisterUserAsync(request.UserName, request.Password);
            await this.cartService.CreateCartAsync(authResponse.UserId);
            return authResponse;
        }
    }
}

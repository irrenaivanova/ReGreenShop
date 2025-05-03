using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ReGreenShop.Application.Common.Identity.Login;
public class LoginCommand : IRequest<AuthResponse>
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IIdentity identity;

        public LoginCommandHandler(IIdentity identity)
        {
            this.identity = identity;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
                    => await this.identity.LoginUserAsync(request.UserName, request.Password);
    }
}

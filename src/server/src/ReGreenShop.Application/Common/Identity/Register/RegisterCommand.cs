using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReGreenShop.Application.Common.Identity.Login;

namespace ReGreenShop.Application.Common.Identity.Register;
public class RegisterCommand : IRequest<AuthResponse>
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly IIdentity identity;

        public RegisterCommandHandler(IIdentity identity)
        {
            this.identity = identity;
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
                    => await this.identity.RegisterUserAsync(request.UserName, request.Password);
    }
}

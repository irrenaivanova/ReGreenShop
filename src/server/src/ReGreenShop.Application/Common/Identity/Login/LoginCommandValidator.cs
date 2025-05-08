using FluentValidation;

namespace ReGreenShop.Application.Common.Identity.Login;
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserName)
            .MaximumLength(128)
            .NotEmpty();
    }
}

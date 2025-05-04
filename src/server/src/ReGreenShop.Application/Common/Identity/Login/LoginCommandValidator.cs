using FluentValidation;

namespace ReGreenShop.Application.Common.Identity.Login;
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        this.RuleFor(x => x.UserName)
            .MaximumLength(128)
            .NotEmpty();

        this.RuleFor(x => x.Password)
            .MinimumLength(6)
            .WithMessage("The password should be minimum 6 characters long");
    }
}

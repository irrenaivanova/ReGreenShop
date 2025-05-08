using FluentValidation;

namespace ReGreenShop.Application.Common.Identity.Register;
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(256).WithMessage("Email length must not exceed 256 characters.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .MinimumLength(6).WithMessage("The password should be minimum 6 characters long");
            //.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            //.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            //.Matches("[0-9]").WithMessage("Password must contain at least one digit.");
    }
}

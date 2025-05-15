using FluentValidation;
using ReGreenShop.Application.Common.Interfaces;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Application.Orders.Commands.MakeAnOrder;
public class MakeAnOrderCommandValidator : AbstractValidator<MakeAnOrderModel>
{
    private readonly IDateTime dateTime;

    public MakeAnOrderCommandValidator(IDateTime dateTime)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(MaxLengthShortName);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(MaxLengthShortName);

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required")
            .MaximumLength(MaxLengthLongName);

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Number is required");

        RuleFor(x => x.PaymentMethodId)
            .NotEmpty().WithMessage("Payment method is required");

        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage("City is required");


        RuleFor(x => x.DeliveryDateTime)
            .NotEmpty().WithMessage("Date and time of delivery is required")
            .Must(BeAtLeastFourHoursFromNow)
            .WithMessage("Delivery time must be at least 4 hours from now.");

        this.dateTime = dateTime;
    }

    private bool BeAtLeastFourHoursFromNow(DateTime deliveryTime)
    {
        return deliveryTime >= this.dateTime.Now.AddHours(4);
    }
}



using FluentValidation;
using MediatR;
using ReGreenShop.Application.Common.Exceptions;

namespace ReGreenShop.Application.Common.Behaviors;
public class RequestValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        => this.validators = validators;

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var failures = this
            .validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            throw new ModelValidationException(failures);
        }

        return next();
    }
}

using FluentValidation.Results;

namespace ReGreenShop.Application.Common.Exceptions;
public class ModelValidationException : Exception
{
    public ModelValidationException()
           : base("One or more validation failures have occurred.")
           => Failures = new Dictionary<string, string[]>();

    public ModelValidationException(List<ValidationFailure> failures)
        : this()
    {
        var failureGroups = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

        foreach (var failureGroup in failureGroups)
        {
            var propertyName = failureGroup.Key;
            var propertyFailures = failureGroup.ToArray();

            Failures.Add(propertyName, propertyFailures);
        }
    }

    public IDictionary<string, string[]> Failures { get; }
}

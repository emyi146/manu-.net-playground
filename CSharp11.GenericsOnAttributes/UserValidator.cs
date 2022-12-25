using FluentValidation;
using FluentValidation.Results;

namespace CSharp11.GenericsOnAttributes;

public class UserValidator : IValidator<User>
{
    public bool CanValidateInstancesOfType(Type type)
    {
        throw new NotImplementedException();
    }

    public IValidatorDescriptor CreateDescriptor()
    {
        throw new NotImplementedException();
    }

    public ValidationResult Validate(User instance)
    {
        throw new NotImplementedException();
    }

    public ValidationResult Validate(IValidationContext context)
    {
        throw new NotImplementedException();
    }

    public Task<ValidationResult> ValidateAsync(User instance, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }
}
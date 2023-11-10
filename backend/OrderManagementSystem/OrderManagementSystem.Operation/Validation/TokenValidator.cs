using FluentValidation;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation.Validation;
public class CreateTokenValidator : AbstractValidator<TokenRequest>
{

    public CreateTokenValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(5).WithMessage("Password is required.");
    }
}

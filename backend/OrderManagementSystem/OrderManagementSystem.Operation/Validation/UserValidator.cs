using FluentValidation;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation.Validation;

public class CreateUserValidator : AbstractValidator<UserRequest>
{

    public CreateUserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Firstname is required.");
        RuleFor(x => x.FirstName).MinimumLength(2).WithMessage("Firstname length min value is 2.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
        RuleFor(x => x.Email).MinimumLength(6).WithMessage("Email length min value is 6.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required.");
        RuleFor(x => x.LastName).MinimumLength(2).WithMessage("LastName length min value is 2.");
    }
}
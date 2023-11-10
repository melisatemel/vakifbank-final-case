using FluentValidation;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation.Validation
{
    public class CreateAddressValidator : AbstractValidator<AddressRequest>
    {
        public CreateAddressValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.AddressLine1).NotEmpty().WithMessage("Address Line 1 is required.");
            RuleFor(x => x.AddressLine1).MinimumLength(20).WithMessage("AddressLine1 length min value is 20.");

        }
    }
}

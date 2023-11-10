using FluentValidation;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation.Validation
{
    public class CreateCardValidator : AbstractValidator<CardRequest>
    {
        public CreateCardValidator()
        {
            RuleFor(x => x.CardHolder).NotEmpty().WithMessage("Card Holder is required.");
            RuleFor(x => x.CardNumber).NotEmpty().WithMessage("Card Number is required.");
            RuleFor(x => x.Cvv).NotEmpty().WithMessage("Cvv is required.");
            RuleFor(x => x.ExpiryDate).NotEmpty().WithMessage("ExpiryDate is required.");
        }
    }
}

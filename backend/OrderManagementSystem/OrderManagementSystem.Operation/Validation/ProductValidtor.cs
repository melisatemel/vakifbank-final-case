using FluentValidation;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation.Validation;

public class ProductValidtor : AbstractValidator<ProductRequest>
{
    public ProductValidtor()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price should be greater than 0.");
        RuleFor(x => x.StockQuantity).GreaterThan(0).WithMessage("Stock quantity should be greater than 0.");

        RuleFor(x => x.Name).MinimumLength(5).WithMessage("Product name length should be at least 5 characters.");
    }
}
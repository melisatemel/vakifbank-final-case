using FluentValidation;
using OrderManagementSystem.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Operation.Validation;

public class ShoppingCartValidator : AbstractValidator<ShoppingCartRequest>
{
    public ShoppingCartValidator()
    {
        RuleFor(x => x.CreatedAt).NotEmpty().WithMessage("CreatedAt is required.");
        RuleFor(x => x.ProductIds).NotEmpty().WithMessage("ProductIds cannot be empty.");
    }
}

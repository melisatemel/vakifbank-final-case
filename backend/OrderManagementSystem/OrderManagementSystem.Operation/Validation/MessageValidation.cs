using FluentValidation;
using OrderManagementSystem.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Operation.Validation;

public class CreateMessageValidator : AbstractValidator<MessageRequest>
{
    public CreateMessageValidator()
    {
        RuleFor(x => x.ChatId).NotEmpty().WithMessage("ChatId is required.");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.");
    }
}


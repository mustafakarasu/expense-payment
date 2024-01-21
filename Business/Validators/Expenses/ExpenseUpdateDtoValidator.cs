using Domain.DataTransferObjects.Expenses;
using FluentValidation;

namespace Business.Validators.Expenses;

public class ExpenseUpdateDtoValidator : AbstractValidator<ExpenseUpdateDto>
{
    public ExpenseUpdateDtoValidator()
    {
        RuleFor(x => x.Description).MaximumLength(450);
        RuleFor(x => x.Amount).NotEmpty().GreaterThan(0).LessThan(Int32.MaxValue);
        RuleFor(x => x.CategoryId).NotEmpty().GreaterThan(0).LessThan(Int32.MaxValue);
        RuleFor(x => x.PaymentMethodId).NotEmpty().GreaterThan(0).LessThan(Int32.MaxValue);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(45);
    }
}
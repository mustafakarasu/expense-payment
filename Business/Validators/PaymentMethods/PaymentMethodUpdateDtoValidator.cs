using Domain.DataTransferObjects.PaymentMethods;
using FluentValidation;

namespace Business.Validators.PaymentMethods;

public class PaymentMethodUpdateDtoValidator : AbstractValidator<PaymentMethodUpdateDto>
{
    public PaymentMethodUpdateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(35);
        RuleFor(x => x.Description).MaximumLength(450);
    }
}
using Domain.Constants;
using Domain.DataTransferObjects.Payments;
using FluentValidation;

namespace Business.Validators.Payments
{
    public class PaymentCreationDtoValidator : AbstractValidator<PaymentCreationDto>
    {
        public PaymentCreationDtoValidator()
        {
            RuleFor(x => x.IsApproved).NotNull();

            When(x => x.IsApproved == false && string.IsNullOrWhiteSpace(x.Description), () =>
            {
                RuleFor(a => a.Description)
                    .NotEmpty()
                    .WithMessage(Messages.RejectionValidationError)
                    .MinimumLength(10).MaximumLength(450);
            });
        }
    }
}

using Domain.Entities;
using FluentValidation;

namespace Business.Validators.PaymentActivities
{
    public class PaymentActivityValidator : AbstractValidator<PaymentActivity>
    {
        public PaymentActivityValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}

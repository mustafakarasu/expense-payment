using Domain.Constants;
using Domain.DataTransferObjects.Employees;
using FluentValidation;

namespace Business.Validators.Users;

public class EmployeeUpdateDtoValidator : AbstractValidator<EmployeeUpdateDto>
{
    public EmployeeUpdateDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.FirstName).MaximumLength(25);
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.LastName).MaximumLength(25);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(256);

        RuleFor(x => x.Email).
            Matches("^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$")
            .WithMessage(Messages.EmailValidationError);
    }
}
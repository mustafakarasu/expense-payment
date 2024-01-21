using Domain.Constants;
using Domain.DataTransferObjects.Employees;
using Domain.DataTransferObjects.Users;
using FluentValidation;

namespace Business.Validators.Users
{
    public class EmployeeCreationDtoValidator : AbstractValidator<EmployeeCreationDto>
    {
        public EmployeeCreationDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.FirstName).MaximumLength(25);
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.LastName).MaximumLength(25);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(256);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);

            RuleFor(x => x.PhoneNumber)
                .Matches("^[0-9]\\d{9}$")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage(Messages.PhoneNumberValidationError);

            RuleFor(x => x.Email).
                Matches("^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$")
                .WithMessage(Messages.EmailValidationError);
        }
    }

    public class UserAuthenticationDtoValidator : AbstractValidator<UserAuthenticationDto>
    {
        public UserAuthenticationDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(256);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);

            RuleFor(x => x.Email).
                Matches("^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$")
                .WithMessage(Messages.EmailValidationError);
        }
    }
}

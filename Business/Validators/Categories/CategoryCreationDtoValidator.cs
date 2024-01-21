using FluentValidation;
using Domain.DataTransferObjects.Categories;

namespace Business.Validators.Categories
{
    public class CategoryCreationDtoValidator : AbstractValidator<CategoryCreationDto>
    {
        public CategoryCreationDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(4).MaximumLength(25);
            RuleFor(x => x.Description).MaximumLength(450);
        }
    }
}

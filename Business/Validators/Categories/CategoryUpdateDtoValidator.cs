using Domain.DataTransferObjects.Categories;
using FluentValidation;

namespace Business.Validators.Categories;

public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
{
    public CategoryUpdateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(25);
        RuleFor(x => x.Description).MaximumLength(450);
    }
}
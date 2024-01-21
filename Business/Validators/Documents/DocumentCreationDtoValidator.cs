using Domain.DataTransferObjects.Documents;
using FluentValidation;

namespace Business.Validators.Documents
{
    public class DocumentCreationDtoValidator : AbstractValidator<DocumentCreationDto>
    {
        public DocumentCreationDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.FolderPath).NotEmpty().MaximumLength(250);
        }
    }
}

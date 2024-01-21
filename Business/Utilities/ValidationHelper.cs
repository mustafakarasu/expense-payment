using FluentValidation;

namespace Business.Utilities
{
    public static class ValidationHelper
    {
        public static void Validate(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity);
            var result = validator.Validate(context);
            if ( !result.IsValid )
            {
                throw new ValidationException(result.Errors);
            }
        }

        public static void Validate<T>(IValidator validator, T entity)
        {
            var context = new ValidationContext<T>(entity);
            var result = validator.Validate(context);
            if ( !result.IsValid )
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}

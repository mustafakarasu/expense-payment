namespace Domain.Constants
{
    public class Messages
    {
        public const string RefreshTokenBadRequest = "Invalid client request. The tokenDto has some invalid values.";
        public const string AlreadyEmail = "This email address is already in use.";
        public const string NoRoleDefinition = "There is no definition for the role you are trying to add.";
        public const string InvalidToken = "Invalid token";
        public const string AuthenticationFailed = "Authentication failed. Wrong user name or password.";
        public const string IncorrectFiles = "Any of the files were not installed correctly.";
        public const string ExpenseNotFound = "No expense with the requested id value was found.";
        public const string ExpenseProcessTransactionError = "A transaction was made with the expense with this id value. It cannot be processed.";
        public const string PaymentNotFound = "No payment was found with the specified id value.";
        public const string CategoryNotFound = "No category was found with the specified id value.";
        public const string PaymentMethodNotFound = "No payment method was found with the specified id value.";
        public const string RejectionValidationError = "For rejection, you must enter a description of at least 10 characters.";
        public const string EmailValidationError = "Please enter an email address in the correct format.";
        public const string CategoryDeletionError = "There are expense transactions that belong to this category. That's why it can't be deleted.";
        public const string PaymentMethodDeletionError = "There are expense transactions that belong to this payment method. That's why it can't be deleted.";
        public const string PhoneNumberValidationError = "Enter the phone number in the correct format. 10 characters and only numbers.";
    }
}

using Domain.DataTransferObjects.Categories;
using Domain.DataTransferObjects.Documents;
using Domain.DataTransferObjects.Payments;
using Domain.DataTransferObjects.PaymentMethods;

namespace Domain.DataTransferObjects.Expenses
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyType { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public CategoryDto Category { get; set; }
        public PaymentMethodDto PaymentMethod { get; set; }
        public PaymentDto Payment { get; set; }
        public List<DocumentDto> Documents { get; set; } = new();
    }
}

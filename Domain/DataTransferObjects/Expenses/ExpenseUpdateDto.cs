using Microsoft.AspNetCore.Http;

namespace Domain.DataTransferObjects.Expenses;

public class ExpenseUpdateDto
{
    public decimal Amount { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public int PaymentMethodId { get; set; }

    public List<IFormFile> Documents { get; set; }
}
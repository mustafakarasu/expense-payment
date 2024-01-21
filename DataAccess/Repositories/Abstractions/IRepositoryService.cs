using DataAccess.Contexts;

namespace DataAccess.Repositories.Abstractions
{
    public interface IRepositoryService
    {
        ICategoryRepository Category { get; }
        IPaymentRepository Payment { get; }
        IExpenseRepository Expense { get; set; }
        IPaymentMethodRepository PaymentMethod { get; }
        IDocumentRepository Document { get; }
        ExpenseContext Context { get; }
        Task SaveAsync();
    }
}

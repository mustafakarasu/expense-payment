using DataAccess.Contexts;
using DataAccess.Repositories.Abstractions;

namespace DataAccess.Repositories;

public class RepositoryService : IRepositoryService
{
    public RepositoryService(ICategoryRepository categoryRepository,
        IPaymentRepository paymentRepository,
        IExpenseRepository expenseRepository,
        IPaymentMethodRepository paymentMethodRepository,
        IDocumentRepository documentRepository,
        ExpenseContext context)
    {
        Category = categoryRepository;
        Payment = paymentRepository;
        Expense = expenseRepository;
        PaymentMethod = paymentMethodRepository;
        Document = documentRepository;
        Context = context;
    }

    public ExpenseContext Context { get; }
    public ICategoryRepository Category { get; }
    public IPaymentRepository Payment { get; }
    public IExpenseRepository Expense { get; set; }
    public IPaymentMethodRepository PaymentMethod { get; }
    public IDocumentRepository Document { get; }

    public async Task SaveAsync()
    {
        await Context.SaveChangesAsync();
    }
}
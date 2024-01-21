using DataAccess.Contexts;
using DataAccess.Repositories.Abstractions;
using Domain.Entities;

namespace DataAccess.Repositories;

public class PaymentMethodRepository : RepositoryBase<PaymentMethod>, IPaymentMethodRepository
{
    private readonly ExpenseContext _expenseContext;

    public PaymentMethodRepository(ExpenseContext expenseContext) : base(expenseContext)
    {
        _expenseContext = expenseContext;
    }
}
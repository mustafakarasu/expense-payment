using DataAccess.Contexts;
using DataAccess.Repositories.Abstractions;
using Domain.Entities;

namespace DataAccess.Repositories;

public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
{
    private readonly ExpenseContext _expenseContext;

    public PaymentRepository(ExpenseContext expenseContext) : base(expenseContext)
    {
        _expenseContext = expenseContext;
    }
}
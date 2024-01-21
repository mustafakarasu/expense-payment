using DataAccess.Contexts;
using DataAccess.Repositories.Abstractions;
using Domain.Entities;

namespace DataAccess.Repositories;

public class ExpenseRepository : RepositoryBase<Expense>, IExpenseRepository
{
    private readonly ExpenseContext _expenseContext;

    public ExpenseRepository(ExpenseContext expenseContext) : base(expenseContext)
    {
        _expenseContext = expenseContext;
    }
}
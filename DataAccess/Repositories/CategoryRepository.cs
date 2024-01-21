using DataAccess.Contexts;
using DataAccess.Repositories.Abstractions;
using Domain.Entities;

namespace DataAccess.Repositories;

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    private readonly ExpenseContext _expenseContext;

    public CategoryRepository(ExpenseContext expenseContext) : base(expenseContext)
    {
        _expenseContext = expenseContext;
    }
}
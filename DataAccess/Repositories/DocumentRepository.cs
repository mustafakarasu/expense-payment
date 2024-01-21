using DataAccess.Contexts;
using DataAccess.Repositories.Abstractions;
using Domain.Entities;

namespace DataAccess.Repositories;

public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
{
    private readonly ExpenseContext _expenseContext;

    public DocumentRepository(ExpenseContext expenseContext) : base(expenseContext)
    {
        _expenseContext = expenseContext;
    }
}
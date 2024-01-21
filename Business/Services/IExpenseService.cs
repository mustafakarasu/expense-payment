using Business.QueryParameters;
using Domain.DataTransferObjects.Expenses;
using Domain.Entities;

namespace Business.Services
{
    public interface IExpenseService
    {
        Task<ExpenseDto> CreateAsync(int userId, ExpenseCreationDto expenseCreationDto);
        Task UpdateAsync(int id, ExpenseUpdateDto expenseUpdateDto);
        Task DeleteAsync(int id);
        Task<ExpenseDto> GetByIdAsync(int id);
        Task<List<ExpenseDto>> GetAllAsync(ExpenseQueryParameter parameter);

        Task<List<ExpenseDto>> GetAllByEmployeeIdAsync(int employeeId, ExpenseQueryParameter queryParameter);
        Task<ExpenseDto> GetByEmployeeIdAsync(int employeeId, int expenseId);
        Task<Expense> CheckAndGetExpenseByIdAsync(int id);
    }
}

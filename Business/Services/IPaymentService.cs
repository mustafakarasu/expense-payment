using Domain.DataTransferObjects.Payments;
using Domain.Entities;

namespace Business.Services;

public interface IPaymentService
{
    Task<PaymentDto> CreateAsync(int expenseId, PaymentCreationDto paymentCreationDto);
    Task<PaymentDto> GetByIdAsync(int id);
    Task<List<PaymentDto>> GetAllAsync();

    Task<PaymentDto> GetForEmployeeByIdAsync(int userId, int id);
    Task<List<PaymentDto>> GetAllForEmployeeAsync(int userId);
    Task<Payment> CheckPaymentByIdAsync(int id);
    Task CheckPaymentByExpenseIdAsync(int expenseId);
}
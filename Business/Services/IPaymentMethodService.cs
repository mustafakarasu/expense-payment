using Domain.DataTransferObjects.PaymentMethods;
using Domain.Entities;

namespace Business.Services;

public interface IPaymentMethodService
{
    Task<PaymentMethodDto> CreateAsync(PaymentMethodCreationDto paymentMethodCreationDto);
    Task UpdateAsync(int id, PaymentMethodUpdateDto paymentMethodUpdateDto);
    Task DeleteAsync(int id);
    Task<PaymentMethodDto> GetByIdAsync(int id);
    Task<List<PaymentMethodDto>> GetAllAsync();
    Task<PaymentMethod> CheckPaymentMethodAndGetByIdAsync(int id);
}
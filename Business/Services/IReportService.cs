using Domain.ReportObjects;

namespace Business.Services
{
    public interface IReportService
    {
        Task<PaymentDensity> GetPaymentDensityForCompany(DateTime startingDate, DateTime lastDate);
        Task<PaymentDensity> GetDailyPaymentDensityForCompany();
        Task<PaymentDensity> GetWeeklyPaymentDensityForCompany();
        Task<PaymentDensity> GetMonthlyPaymentDensityForCompany();


        Task<PaymentDensity> GetPaymentDensityForEmployee(int userId, DateTime startingDate, DateTime lastDate);
        Task<PaymentDensity> GetDailyPaymentDensityForEmployee(int userId);
        Task<PaymentDensity> GetWeeklyPaymentDensityForEmployee(int userId);
        Task<PaymentDensity> GetMonthlyPaymentDensityForEmployee(int userId);
    }
}

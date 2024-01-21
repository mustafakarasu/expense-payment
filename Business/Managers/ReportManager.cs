using System.Data;
using Business.Services;
using Dapper;
using Domain.ReportObjects;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Business.Managers;

public class ReportManager : IReportService, IDisposable
{
    private readonly IDbConnection _connection;

    public ReportManager(IConfiguration configuration)
    {
        _connection = new SqlConnection(configuration.GetConnectionString("SqlServer"));
    }

    public async Task<PaymentDensity> GetPaymentDensityForCompany(DateTime startingDate, DateTime lastDate)
    {
        var parameters = new DynamicParameters();
        parameters.Add("startingDate", startingDate);
        parameters.Add("lastDate", lastDate);
        var result = await _connection.QueryFirstOrDefaultAsync<PaymentDensity>(
            "dbo.sp_PaymentDensityForCompany", parameters, commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<PaymentDensity> GetDailyPaymentDensityForCompany()
    {
        return await GetPaymentDensityForCompany(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(1));
    }

    public async Task<PaymentDensity> GetWeeklyPaymentDensityForCompany()
    {
        return await GetPaymentDensityForCompany(DateTime.UtcNow.Date.AddDays(-6), DateTime.UtcNow.Date.AddDays(1));
    }

    public async Task<PaymentDensity> GetMonthlyPaymentDensityForCompany()
    {
        return await GetPaymentDensityForCompany(DateTime.UtcNow.Date.AddMonths(-1), DateTime.UtcNow.Date.AddDays(1));
    }

    public async Task<PaymentDensity> GetPaymentDensityForEmployee(int userId, DateTime startingDate, DateTime lastDate)
    {
        var parameters = new DynamicParameters();
        parameters.Add("userId", userId);
        parameters.Add("startingDate", startingDate);
        parameters.Add("lastDate", lastDate);
        var result = await _connection.QueryFirstOrDefaultAsync<PaymentDensity>(
            "dbo.sp_PaymentDensityForEmployee", parameters, commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<PaymentDensity> GetDailyPaymentDensityForEmployee(int userId)
    {
        return await GetPaymentDensityForEmployee(userId, DateTime.UtcNow.Date ,DateTime.UtcNow.Date.AddDays(1));
    }

    public async Task<PaymentDensity> GetWeeklyPaymentDensityForEmployee(int userId)
    {
        return await GetPaymentDensityForEmployee(userId, DateTime.UtcNow.Date.AddDays(-6), DateTime.UtcNow.Date.AddDays(1));
    }

    public async Task<PaymentDensity> GetMonthlyPaymentDensityForEmployee(int userId)
    {
        return await GetPaymentDensityForEmployee(userId, DateTime.UtcNow.Date.AddMonths(-1), DateTime.UtcNow.Date.AddDays(1));
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
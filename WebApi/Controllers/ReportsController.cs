using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/daily")]
        public async Task<IActionResult> GetDailyReportForCompany()
        {
            var result = await _reportService.GetDailyPaymentDensityForCompany();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/weekly")]
        public async Task<IActionResult> GetWeeklyReportForCompany()
        {
            var result = await _reportService.GetWeeklyPaymentDensityForCompany();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/monthly")]
        public async Task<IActionResult> GetMonthlyReportForCompany()
        {
            var result = await _reportService.GetMonthlyPaymentDensityForCompany();
            return Ok(result);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("employee/daily")]
        public async Task<IActionResult> GetDailyReportForEmployee()
        {
            var result = await _reportService.GetDailyPaymentDensityForEmployee(User.GetUserId());
            return Ok(result);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("employee/weekly")]
        public async Task<IActionResult> GetWeeklyReportForEmployee()
        {
            var result = await _reportService.GetWeeklyPaymentDensityForEmployee(User.GetUserId());
            return Ok(result);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("employee/monthly")]
        public async Task<IActionResult> GetMonthlyReportForEmployee()
        {
            var result = await _reportService.GetMonthlyPaymentDensityForEmployee(User.GetUserId());
            return Ok(result);
        }
    }
}

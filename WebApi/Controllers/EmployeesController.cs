using Business.Services;
using Domain.DataTransferObjects.Expenses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.QueryParameters;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    [Route("api/employees")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IExpenseService _expenseService;
        private readonly IPaymentService _paymentService;

        public EmployeesController(IEmployeeService employeeService, IExpenseService expenseService, IPaymentService paymentService)
        {
            _employeeService = employeeService;
            _expenseService = expenseService;
            _paymentService = paymentService;
        }

        [HttpGet("expenses")]
        public async Task<IActionResult> GetExpenses([FromQuery] ExpenseQueryParameter queryParameter)
        {
            var result = await _expenseService.GetAllByEmployeeIdAsync(User.GetUserId(), queryParameter);
            return Ok(result);
        }

        [HttpGet("expenses/{expenseId:int}", Name = "GetExpenseById")]
        public async Task<IActionResult> GetExpenseById(int expenseId)
        {
            var result = await _expenseService.GetByEmployeeIdAsync(User.GetUserId(), expenseId);
            return Ok(result);
        }

        [HttpPost("expenses")]
        public async Task<IActionResult> CreateExpense([FromForm] ExpenseCreationDto expenseCreationDto)
        {
            var result = await _expenseService.CreateAsync(User.GetUserId(), expenseCreationDto);

            return CreatedAtRoute("GetExpenseById", new { expenseId = result.Id }, result);
        }

        [HttpPut("expenses/{expenseId:int}")]
        public async Task<IActionResult> UpdateExpense(int expenseId, [FromForm] ExpenseUpdateDto expenseUpdateDto)
        {
            await _expenseService.UpdateAsync(expenseId, expenseUpdateDto);

            return NoContent();
        }

        [HttpDelete("expenses/{expenseId:int}")]
        public async Task<IActionResult> DeleteExpense(int expenseId)
        {
            await _expenseService.DeleteAsync(expenseId);
            return NoContent();
        }

        [HttpGet("payments")]
        public async Task<IActionResult> GetPayments()
        {
            var result = await _paymentService.GetAllForEmployeeAsync(User.GetUserId());
            return Ok(result);
        }

        [HttpGet("payments/{paymentId:int}", Name = "GetPaymentByExpenseId")]
        public async Task<IActionResult> GetPaymentByExpenseId(int paymentId)
        {
            var result = await _paymentService.GetForEmployeeByIdAsync(User.GetUserId(), paymentId);
            return Ok(result);
        }
    }
}

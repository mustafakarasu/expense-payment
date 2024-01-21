using Business.QueryParameters;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/expenses")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenses([FromQuery] ExpenseQueryParameter queryParameter)
        {
            var result = await _expenseService.GetAllAsync(queryParameter);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetExpense(int id)
        {
            var result = await _expenseService.GetByIdAsync(id);
            return Ok(result);
        }
    }
}

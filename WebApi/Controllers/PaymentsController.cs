using Business.Services;
using Domain.DataTransferObjects.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/payments")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
            var result = await _paymentService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var result = await _paymentService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("{expenseId:int}")]
        public async Task<IActionResult> GetPayment(int expenseId, [FromBody] PaymentCreationDto paymentCreationDto)
        {
            var result = await _paymentService.CreateAsync(expenseId, paymentCreationDto);
            return Ok(result);
        }
    }
}

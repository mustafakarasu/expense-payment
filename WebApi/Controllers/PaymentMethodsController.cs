using Business.Services;
using Domain.DataTransferObjects.PaymentMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/payment-methods")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodsController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }


        [HttpGet]
        public async Task<OkObjectResult> GetPaymentMethods()
        {
            var result = await _paymentMethodService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "GetPaymentMethodById")]
        public async Task<IActionResult> GetPaymentMethodById(int id)
        {
            var result = await _paymentMethodService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<CreatedAtRouteResult> CreatePaymentMethod([FromBody] PaymentMethodCreationDto paymentMethodCreationDto)
        {
            var entityPaymentMethod = await _paymentMethodService.CreateAsync(paymentMethodCreationDto);

            return CreatedAtRoute("GetPaymentMethodById", new { id = entityPaymentMethod.Id }, entityPaymentMethod);
        }

        [HttpPut("{id:int}")]
        public async Task<NoContentResult> UpdatePaymentMethod(int id, [FromBody] PaymentMethodUpdateDto paymentMethodUpdateDto)
        {
            await _paymentMethodService.UpdateAsync(id, paymentMethodUpdateDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<NoContentResult> DeletePaymentMethod(int id)
        {
            await _paymentMethodService.DeleteAsync(id);
            return NoContent();
        }
    }
}

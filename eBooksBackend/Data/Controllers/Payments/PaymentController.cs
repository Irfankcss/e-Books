using eBooksBackend.Data.Controllers.Payments;
using eBooksBackend.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace eBooksBackend.Data.Controllers.Payments
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDto paymentDto)
        {
            try
            {
                if (paymentDto == null || paymentDto.BookId <= 0 || paymentDto.UserId <= 0)
                {
                    return BadRequest("Invalid payment data");
                }

                var payment = await _paymentService.ProcessPaymentAsync(
                    paymentDto.OrderId,
                    paymentDto.PaymentId,
                    paymentDto.Status,
                    paymentDto.Amount
                );

                return Ok(new { 
                    Message = "Payment processed successfully",
                    PaymentId = payment.Id,
                    Status = payment.PaymentStatus
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentByOrderId(int orderId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByOrderIdAsync(orderId);
                if (payment == null)
                {
                    return NotFound("Payment not found");
                }

                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
} 
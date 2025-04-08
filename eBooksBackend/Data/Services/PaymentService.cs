using eBooksBackend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace eBooksBackend.Data.Services
{
    public class PaymentService
    {
        private readonly eBookStoreDbContext _dbContext;

        public PaymentService(eBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Payment> ProcessPaymentAsync(int orderId, string paymentId, string status, float amount)
        {
            var order = await _dbContext.orders
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            var payment = new Payment
            {
                OrderId = orderId,
                Amount = amount,
                PaymentMethod = "PayPal",
                PaymentStatus = status,
                PaymentDate = DateTime.UtcNow
            };

            _dbContext.payments.Add(payment);
            order.Status = status;
            await _dbContext.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment> GetPaymentByOrderIdAsync(int orderId)
        {
            return await _dbContext.payments
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }
    }
} 
using eBooksBackend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eBooksBackend.Data.Controllers.Orders
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly eBookStoreDbContext _dbContext;
        public OrderController(eBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] OrderCreateRequest request)
        {
            var cart = await _dbContext.carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.eBook)
                .FirstOrDefaultAsync(c => c.Id == request.CartId && c.UserId == request.UserId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                return BadRequest("Cart is empty or does not exist.");

            float totalAmount = cart.CartItems.Sum(item => item.eBook.Price * item.Quantity);

            var order = new Order
            {
                UserId = cart.UserId,
                TotalAmount = totalAmount,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OrderDate = DateTime.UtcNow,
                OrderItems = cart.CartItems.Select(item => new OrderItem
                {
                    eBookId = item.eBookId,
                    Quantity = item.Quantity,
                    Price = item.eBook.Price
                }).ToList()
            };

            _dbContext.orders.Add(order);
            _dbContext.cartItems.RemoveRange(cart.CartItems);

            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "Order created successfully.", OrderId = order.Id });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            var order = await _dbContext.orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.eBook)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound("Order not found.");

            var orderDTO = new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    eBookId = oi.eBookId,
                    eBookTitle = oi.eBook.Title,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };

            return Ok(orderDTO);
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetAllOrders()
        {
            var orders = await _dbContext.orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.eBook)
                .ToListAsync();

            var orderDTOs = orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    eBookId = oi.eBookId,
                    eBookTitle = oi.eBook.Title,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            }).ToList();

            return Ok(orderDTOs);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var order = await _dbContext.orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound("Order not found.");

            _dbContext.orderItems.RemoveRange(order.OrderItems);
            _dbContext.orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return Ok("Order deleted successfully.");
        }

        [HttpPut("updateStatus")]
        public async Task<ActionResult> UpdateOrderStatus(int id,string Status)
        {
            var order = await _dbContext.orders.FindAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            if (string.IsNullOrWhiteSpace(Status))
            {
                return BadRequest("Status cannot be empty.");
            }

            order.Status = Status;
            order.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "Order status updated successfully.", OrderId = order.Id });
        }
    }



    public class OrderCreateRequest
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
    }

}


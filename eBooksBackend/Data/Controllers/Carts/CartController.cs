using eBooksBackend.Data.Controllers.CartItems;
using eBooksBackend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eBooksBackend.Data.Controllers.Carts
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly eBookStoreDbContext _dbContext;

        public CartController(eBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("byUserId")]
        public async Task<ActionResult<CartDto>> GetCartByUserId([FromQuery] int userId)
        {
            var cart = await _dbContext.carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.eBook) // Include eBook details
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return NotFound("Cart not found for this user.");

            var cartDto = new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                Items = cart.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    eBookId = ci.eBookId,
                    eBookTitle = ci.eBook.Title,
                    Quantity = ci.Quantity,
                    CreatedAt = ci.CreatedAt,
                    UpdatedAt = ci.UpdatedAt
                }).ToList()
            };

            return Ok(cartDto);
        }

        [HttpDelete("byUserId")]
        public async Task<ActionResult> DeleteCartByUserId([FromQuery] int userId)
        {
            var cart = await _dbContext.carts
                .Include(c => c.CartItems) 
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound("Cart does not exist for this user.");

            _dbContext.carts.Remove(cart);
            await _dbContext.SaveChangesAsync();

            return Ok("Cart deleted successfully.");
        }
    }
}

using eBooksBackend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eBooksBackend.Data.Controllers.CartItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly eBookStoreDbContext _dbContext;

        public CartItemController(eBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet("byCartId")]
        public async Task<ActionResult<List<CartItem>>> GetCartItemsByCartId([FromQuery] int cartId)
        {
            var cartExists = await _dbContext.carts.AnyAsync(c => c.Id == cartId);
            if (!cartExists) return NotFound("Cart not found.");

            var cartItems = await _dbContext.cartItems
                .Where(ci => ci.CartId == cartId)
                .Include(ci => ci.eBook) 
                .ToListAsync();

            return Ok(cartItems);
        }

        [HttpPost]
        public async Task<ActionResult> AddItemToCart([FromBody] CartItemCreateRequest request)
        {
            var cartExists = await _dbContext.carts.AnyAsync(c => c.Id == request.CartId);
            var bookExists = await _dbContext.eBook.AnyAsync(b => b.Id == request.eBookId);

            if (!cartExists) return NotFound("Cart not found.");
            if (!bookExists) return NotFound("eBook not found.");

            var cartItem = new CartItem
            {
                CartId = request.CartId,
                eBookId = request.eBookId,
                Quantity = request.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.cartItems.Add(cartItem);
            await _dbContext.SaveChangesAsync();

            return Ok("Item added to cart successfully.");
        }

        [HttpPut("updateQuantity")]
        public async Task<ActionResult> UpdateCartItemQuantity([FromBody] CartItemUpdateRequest request)
        {
            var cartItem = await _dbContext.cartItems.FindAsync(request.CartItemId);
            if (cartItem == null) return NotFound("Cart item not found.");

            cartItem.Quantity = request.Quantity > 0 ? request.Quantity : cartItem.Quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return Ok("Cart item quantity updated successfully.");
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCartItem([FromQuery] int cartItemId)
        {
            var cartItem = await _dbContext.cartItems.FindAsync(cartItemId);
            if (cartItem == null) return NotFound("Cart item not found.");

            _dbContext.cartItems.Remove(cartItem);
            await _dbContext.SaveChangesAsync();

            return Ok("Cart item deleted successfully.");
        }
    }

    public class CartItemCreateRequest
    {
        public int CartId { get; set; }
        public int eBookId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartItemUpdateRequest
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
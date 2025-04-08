using eBooksBackend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eBooksBackend.Data.Controllers.Reviews
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly eBookStoreDbContext _dbContext;
        public ReviewController(eBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("byBookId")]
        public async Task<ActionResult<List<Review>>> GetReviewsByBookId([FromQuery] int bookId)
        {
            var reviews = await _dbContext.reviews
                .Where(r => r.eBookId == bookId)
                .Include(r => r.User)
                .ToListAsync();

            return Ok(reviews);
        }

        [HttpGet("byUserId")]
        public async Task<ActionResult<List<Review>>> GetReviewsByUserId([FromQuery] int userId)
        {
            var reviews = await _dbContext.reviews
                .Where(r => r.UserId == userId)
                .Include(r => r.eBook)
                .ToListAsync();

            return Ok(reviews);
        }

        [HttpPost]
        public async Task<ActionResult> CreateReview([FromBody] ReviewCreateRequest newReview)
        {
            var userExists = await _dbContext.users.AnyAsync(u => u.Id == newReview.UserId);
            var bookExists = await _dbContext.eBook.AnyAsync(b => b.Id == newReview.eBookId);

            if (!userExists) return NotFound("User does not exist.");
            if (!bookExists) return NotFound("Book does not exist.");

            var review = new Review
            {
                Content = newReview.Content,
                Rating = newReview.Rating,
                CreatedAt = DateTime.UtcNow,
                UserId = newReview.UserId,
                eBookId = newReview.eBookId
            };

            _dbContext.reviews.Add(review);
            await _dbContext.SaveChangesAsync();

            return Ok("Review created successfully.");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateReview([FromQuery] ReviewUpdateRequest updatedReview)
        {
            var existingReview = await _dbContext.reviews.FindAsync(updatedReview.Id);
            if (existingReview == null) return NotFound("Review not found.");

            existingReview.Content = string.IsNullOrWhiteSpace(updatedReview.Content)?existingReview.Content:updatedReview.Content;

            existingReview.Rating = updatedReview.Rating ?? existingReview.Rating;

            await _dbContext.SaveChangesAsync();

            return Ok("Review updated successfully.");
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteReview([FromQuery] int id)
        {
            var review = await _dbContext.reviews.FindAsync(id);
            if (review == null) return NotFound("Review not found.");

            _dbContext.reviews.Remove(review);
            await _dbContext.SaveChangesAsync();

            return Ok("Review deleted successfully.");
        }
    }

    public class ReviewCreateRequest
    {
        public string Content { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }
        public int eBookId { get; set; }
    }

    public class ReviewUpdateRequest
    {
        public int Id { get; set; }
        public string Content { get; set; } = "";
        public int? Rating { get; set; }
    }
}


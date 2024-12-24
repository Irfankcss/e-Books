using eBooksBackend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eBooksBackend.Data.Controllers.eBookRatings
{
    [Route("api/[controller]")]
    [ApiController]
    public class eBookRatingController : ControllerBase
    {
        private readonly eBookStoreDbContext _context;

        public eBookRatingController(eBookStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRatings()
        {
            var ratings = await _context.eBookRatings
                .Include(r => r.User)
                .Include(r => r.eBook)
                .ToListAsync();

            return Ok(ratings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRatingById(int id)
        {
            var rating = await _context.eBookRatings
                .Include(r => r.User)
                .Include(r => r.eBook)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rating == null)
            {
                return NotFound(new { message = "Rating not found." });
            }

            return Ok(rating);
        }

        [HttpGet("eBook/{eBookId}")]
        public async Task<IActionResult> GetRatingsForEBook(int eBookId)
        {
            var ratings = await _context.eBookRatings
                .Where(r => r.eBookId == eBookId)
                .Include(r => r.User)
                .ToListAsync();

            return Ok(ratings);
        }

        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] eBookRating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eBookExists = await _context.eBook.AnyAsync(e => e.Id == rating.eBookId);
            if (!eBookExists)
            {
                return NotFound(new { message = "eBook not found." });
            }

            var userExists = await _context.users.AnyAsync(u => u.Id == rating.userID);
            if (!userExists)
            {
                return NotFound(new { message = "User not found." });
            }

            _context.eBookRatings.Add(rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRatingById), new { id = rating.Id }, rating);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            var rating = await _context.eBookRatings.FindAsync(id);
            if (rating == null)
            {
                return NotFound(new { message = "Rating not found." });
            }

            _context.eBookRatings.Remove(rating);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] eBookRating updatedRating)
        {
            if (id != updatedRating.Id)
            {
                return BadRequest(new { message = "Rating ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRating = await _context.eBookRatings.FindAsync(id);
            if (existingRating == null)
            {
                return NotFound(new { message = "Rating not found." });
            }

            existingRating.Rating = updatedRating.Rating;
            existingRating.Review = updatedRating.Review;

            _context.eBookRatings.Update(existingRating);
            await _context.SaveChangesAsync();

            return Ok(existingRating);
        }
    }
}

using eBooksBackend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eBooksBackend.Data.Controllers.eBookCategories
{
    [ApiController]
    [Route("api/[controller]")]
    public class eBookCategoryController : ControllerBase
    {
        private readonly eBookStoreDbContext _dbContext;

        public eBookCategoryController(eBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<eBookCategory>>> GetAlleBookCategories()
        {
            var eBookCategories = await _dbContext.eBookCategories
                .Include(e => e.eBook)
                .Include(c => c.Category)
                .ToListAsync();
            return Ok(eBookCategories);
        }

        [HttpGet("byBook/{bookId}")]
        public async Task<ActionResult<List<eBookCategory>>> GetByBookId(int bookId)
        {
            var eBookCategories = await _dbContext.eBookCategories
                .Include(e => e.eBook)
                .Include(c => c.Category)
                .Where(ec => ec.eBooksId == bookId)
                .ToListAsync();

            if (eBookCategories == null || !eBookCategories.Any()) return NotFound();

            return Ok(eBookCategories);
        }

        [HttpGet("byCategory/{categoryId}")]
        public async Task<ActionResult<List<eBookCategory>>> GetByCategoryId(int categoryId)
        {
            var eBookCategories = await _dbContext.eBookCategories
                .Include(e => e.eBook)
                .Include(c => c.Category)
                .Where(ec => ec.CategoryId == categoryId)
                .ToListAsync();

            if (eBookCategories == null || !eBookCategories.Any()) return NotFound();

            return Ok(eBookCategories);
        }

        [HttpPost]
        public async Task<ActionResult> CreateeBookCategory([FromQuery] int bookId, [FromQuery] int categoryId)
        {
            var bookExists = await _dbContext.eBook.AnyAsync(e => e.Id == bookId);
            if (!bookExists)
                return NotFound("Book with the specified ID does not exist.");

            var categoryExists = await _dbContext.categories.AnyAsync(c => c.Id == categoryId);
            if (!categoryExists)
                return NotFound("Category with the specified ID does not exist.");

            var existingRelation = await _dbContext.eBookCategories
                .AnyAsync(ec => ec.eBooksId == bookId && ec.CategoryId == categoryId);
            if (existingRelation)
                return Conflict("This book-category relationship already exists.");

            var eBookCategory = new eBookCategory
            {
                eBooksId = bookId,
                CategoryId = categoryId
            };

            _dbContext.eBookCategories.Add(eBookCategory);
            await _dbContext.SaveChangesAsync();

            return Ok("Book-category relationship created successfully.");
        }


        [HttpDelete("byBookAndCategory")]
        public async Task<ActionResult> DeleteByBookAndCategory([FromQuery] int bookId, [FromQuery] int categoryId)
        {
            var eBookCategory = await _dbContext.eBookCategories
                .FirstOrDefaultAsync(ec => ec.eBooksId == bookId && ec.CategoryId == categoryId);

            if (eBookCategory == null) return NotFound("The specified book-category relationship does not exist.");

            _dbContext.eBookCategories.Remove(eBookCategory);
            await _dbContext.SaveChangesAsync();

            return Ok("Book-category relationship deleted successfully.");
        }
    }
}

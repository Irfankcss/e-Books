using eBooksBackend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace eBooksBackend.Data.Controllers.eBook
{
    [Route("api/[controller]")]
    [ApiController]
    public class eBookController : ControllerBase
    {
        private readonly eBookStoreDbContext _dbContext;

        public eBookController(eBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Models.eBook>>> GetAllEBooks()
        {
            var ebooks = await _dbContext.eBook.ToListAsync();
            return Ok(ebooks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.eBook>> GetEBookById(int id)
        {
            var ebook = await _dbContext.eBook.FindAsync(id);

            if (ebook == null)
            {
                return NotFound($"eBook with ID {id} not found.");
            }

            return Ok(ebook);
        }

        [HttpPost]
        public async Task<ActionResult<Models.eBook>> CreateEBook([FromBody] Models.eBook ebook)
        {
            if (ebook == null)
            {
                return BadRequest("Invalid eBook data.");
            }

            ebook.CreatedAt = DateTime.UtcNow;

            _dbContext.eBook.Add(ebook);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEBookById), new { id = ebook.Id }, ebook);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEBook(int id, [FromBody] Models.eBook updatedEBook)
        {
            var existingEBook = await _dbContext.eBook.FindAsync(id);
            if (existingEBook == null)
            {
                return NotFound($"eBook with ID {id} not found.");
            }

            existingEBook.Title = updatedEBook.Title;
            existingEBook.Author = updatedEBook.Author;
            existingEBook.Description = updatedEBook.Description;
            existingEBook.Price = updatedEBook.Price;
            existingEBook.Path = updatedEBook.Path;
            existingEBook.Cover = updatedEBook.Cover;
            existingEBook.PublishedDate = updatedEBook.PublishedDate;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEBook(int id)
        {
            var ebook = await _dbContext.eBook.FindAsync(id);

            if (ebook == null)
            {
                return NotFound($"eBook with ID {id} not found.");
            }

            _dbContext.eBook.Remove(ebook);
            await _dbContext.SaveChangesAsync();

            return Ok($"eBook with ID {id} deleted.");
        }
    }
}

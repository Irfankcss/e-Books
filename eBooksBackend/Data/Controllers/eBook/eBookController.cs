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
        public async Task<ActionResult<List<Models.eBook>>> GeteBooks()
        {
            if (_dbContext.eBook == null)
            {
                return NotFound("Database table is empty or null.");
            }

            var list = await _dbContext.eBook.ToListAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<Models.eBook>> CreateEBook(Models.eBook ebook)
        {
            _dbContext.eBook.Add(ebook);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GeteBooks), new { id = ebook.Id }, ebook);
        }
    }
}

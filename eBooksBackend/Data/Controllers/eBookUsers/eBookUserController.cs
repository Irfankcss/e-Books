using eBooksBackend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eBooksBackend.Data.Controllers.eBookUsers
{
    [Route("api/[controller]")]
    [ApiController]
    public class eBookUserController : ControllerBase
    {
        private readonly eBookStoreDbContext _dbContext;

        public eBookUserController(eBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("purchase")]
        public async Task<ActionResult> PurchaseBook([FromBody] PurchaseRequest request)
        {
            var user = await _dbContext.users.FindAsync(request.UserId);
            var ebook = await _dbContext.eBook.FindAsync(request.EbookId);

            if (user == null || ebook == null)
            {
                return NotFound("User or Ebook not found.");
            }

            var eBookUser = new eBookUser
            {
                UserId = request.UserId,
                EbookId = request.EbookId,
                PurchaseDate = DateTime.Now,
                Format = request.Format
            };

            _dbContext.eBookUsers.Add(eBookUser);
            await _dbContext.SaveChangesAsync();

            return Ok("Book purchased successfully.");
        }

        [HttpGet("{userId}/books")]
        public async Task<ActionResult> GetUserBooks(int userId)
        {
            var books = await _dbContext.eBookUsers
                .Where(ue => ue.UserId == userId)
                .Include(ue => ue.Ebook) 
                .Select(ue => new
                {
                    ue.Ebook.Id,
                    ue.Ebook.Title,
                    ue.Ebook.Author,
                    ue.Ebook.Cover,
                    ue.PurchaseDate,
                    ue.Format
                })
                .ToListAsync();

            if (!books.Any())
            {
                return NotFound("No books found for this user.");
            }

            return Ok(books);
        }
    }

    public class PurchaseRequest
    {
        public int UserId { get; set; }
        public int EbookId { get; set; }
        public string Format { get; set; }
    }
}


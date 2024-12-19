using eBooksBackend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eBooksBackend.Data.Controllers.Categories
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController :ControllerBase
    {
        private readonly eBookStoreDbContext _dbContext;
        public CategoryController(eBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            var categories = await _dbContext.categories.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _dbContext.categories.FindAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
        {
            _dbContext.categories.Add(category);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCategory([FromQuery] CategoryUpdateRequest updatedCategory)
        {
            var existingCategory = await _dbContext.categories.FindAsync(updatedCategory.Id);
            if (existingCategory == null) return NotFound();

            existingCategory.Name = updatedCategory.Name==""?existingCategory.Name:updatedCategory.Name;
            existingCategory.Description = updatedCategory.Description==""?existingCategory.Description:updatedCategory.Description;
            existingCategory.Image = string.IsNullOrEmpty(updatedCategory.Image)?existingCategory.Image:updatedCategory.Image;

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = await _dbContext.categories.FindAsync(id);
            if (category == null) return NotFound();

            _dbContext.categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}

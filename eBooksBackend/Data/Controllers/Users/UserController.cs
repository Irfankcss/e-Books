using eBooksBackend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace eBooksBackend.Data.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly eBookStoreDbContext _dbContext;
        public UserController(eBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> getUser(int? UserID, string? UserName)
        {
            if (!_dbContext.users.Any())
            {
                return NotFound("No users found.");
            }

            List<User> lista = new List<User>();
            if (UserID == null && (UserName == "" || UserName == null))
            {
                lista = new List<User>();
                lista = await _dbContext.users.ToListAsync();

            }
            else if (UserID == null)
            {
                lista = await _dbContext.users.Where(x => x.Username.ToLower().StartsWith(UserName.ToLower())).ToListAsync();

            }
            else if (string.IsNullOrWhiteSpace(UserName))
            {
                lista = await _dbContext.users.Where(x => x.Id == UserID).ToListAsync();
            }
            return Ok(lista);
        }


        [HttpDelete]
        public async Task<ActionResult<User>> deleteUser(int UserId)
        {
            if (!_dbContext.users.Any())
            {
                return NotFound("No users found.");
            }
            User user = await _dbContext.users.FirstOrDefaultAsync(x => x.Id == UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            _dbContext.users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return Accepted(user);

        }

        [HttpPut]
        public async Task<ActionResult<User>> updateUser([FromQuery] UpdateUserRequest upUser)
        {
            if (!_dbContext.users.Any())
                return NotFound("No users found.");

            User user = await _dbContext.users.FirstOrDefaultAsync(x => x.Id == upUser.Id);
            if (user == null)
                return NotFound("User not found");

            user.Username = upUser.Username == null ? user.Username : upUser.Username;
            user.PasswordHash = upUser.PasswordHash == null ? user.PasswordHash : upUser.PasswordHash;
            user.BirthDate = upUser.BirthDate == DateTime.MinValue ? user.BirthDate : upUser.BirthDate;
            user.Email = upUser.Email == null ? user.Email : upUser.Email;
            user.Role = upUser.Role == null ? user.Role : upUser.Role;
            _dbContext.users.Update(user);
            await _dbContext.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.PasswordHash))
            {
                return BadRequest("Username and Password are required.");
            }

            if (_dbContext.users.Any(u => u.Username == user.Username))
            {
                return Conflict("A user with this username already exists.");
            }

            if (_dbContext.users.Any(u => u.Email == user.Email))
            {
                return Conflict("A user with this email already exists.");
            }

            user.PasswordHash = HashPassword(user.PasswordHash);
            user.CreatedAt = DateTime.UtcNow;
            user.Role = string.IsNullOrEmpty(user.Role)||user.Role=="string" ? "User" : user.Role;

            _dbContext.users.Add(user);
            await _dbContext.SaveChangesAsync();

            var cart = new Cart
            {
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.carts.Add(cart);
            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "User created successfully.", UserId = user.Id });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}

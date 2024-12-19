using eBooksBackend.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (string.IsNullOrEmpty(createUserDto.Username) || string.IsNullOrEmpty(createUserDto.Password))
            {
                return BadRequest("Username and Password are required.");
            }

            if (_dbContext.users.Any(u => u.Username == createUserDto.Username))
            {
                return Conflict("A user with this username already exists.");
            }

            if (_dbContext.users.Any(u => u.Email == createUserDto.Email))
            {
                return Conflict("A user with this email already exists.");
            }

            var user = new User
            {
                Username = createUserDto.Username,
                PasswordHash = HashPassword(createUserDto.Password),
                Email = createUserDto.Email,
                BirthDate = createUserDto.BirthDate,
                Role = "User", 
                CreatedAt = DateTime.UtcNow
            };

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

            var token = GenerateJwtToken(user);

            var userResponse = new CreateUserResponse
            {
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                BirthDate = user.BirthDate,
                Id = user.Id,
                Role = user.Role
            };

            return Ok(new { Message = "User created successfully.", Token = token, User = userResponse });
        }

        //[HttpPost("SignIn")]
        //public async Task<ActionResult> SignIn([FromBody] User login)
        //{
        //    var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Username == login.Username);

        //    if (user == null || user.PasswordHash != HashPassword(login.PasswordHash))
        //    {
        //        return Unauthorized("Invalid username or password.");
        //    }

        //    var token = GenerateJwtToken(user);

        //    return Ok(new { Message = "Login successful.", Token = token });
        //}

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourVerySecureSuperLongSecretKeyThatIsAtLeast32Characters!"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var token = new JwtSecurityToken(
                issuer: "eBooksBackend",
                audience: "eBooksBackend",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
    }
}

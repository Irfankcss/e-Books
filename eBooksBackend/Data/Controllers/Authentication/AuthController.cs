using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using eBooksBackend.Data;
using eBooksBackend.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using eBooksBackend.Data.Controllers.Users;

namespace eBooksBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly eBookStoreDbContext _dbContext;

        public AuthController(eBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GoogleLogin")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Email and Password are required.");
            }

            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            var hashedPassword = (HashPassword(loginRequest.Password));
            if (user.PasswordHash != hashedPassword)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = GenerateJwtToken(user);
            var userResponse = new CreateUserResponse
            {
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                BirthDate = user.BirthDate,
                Id = user.Id,
                Role = user.Role
                , Photo= user.Photo
            };

            return Ok(new {User = userResponse, Token = token, Message = "Login successful" });
        }

        [HttpGet("GoogleResponse")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync("Google");
            if (!result.Succeeded)
            {
                return Unauthorized("Google authentication failed.");
            }

            var email = result.Principal.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var name = result.Principal.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Unable to retrieve email from Google.");
            }

            var existingUser = _dbContext.users.FirstOrDefault(u => u.Email == email);
            if (existingUser == null)
            {
                var newUser = new User
                {
                    Username = email.Split('@')[0],
                    Email = email,
                    Role = "User",
                    CreatedAt = DateTime.UtcNow,
                    PasswordHash = "string"
                };

                _dbContext.users.Add(newUser);
                await _dbContext.SaveChangesAsync();

                existingUser = newUser;
            }
            var userResponse = new CreateUserResponse
            {
                Username = existingUser.Username,
                Email = existingUser.Email,
                CreatedAt = existingUser.CreatedAt,
                BirthDate = existingUser.BirthDate,
                Id = existingUser.Id,
                Role = existingUser.Role
            };

            var token = GenerateJwtToken(existingUser);

            return Ok(new
            {
                Message = "Google login successful.",
                Token = token,
                User = userResponse
            });
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LetiPuniLastaSmoBarem32KarakteraTankerNaVrhBrdaShriLankaVrbaMrda123456!"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: "eBooksBackend",
                audience: "eBooksBackend",
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials);

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

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}

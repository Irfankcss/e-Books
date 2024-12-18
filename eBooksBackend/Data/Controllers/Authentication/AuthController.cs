using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using eBooksBackend.Data;
using eBooksBackend.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;

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

            var token = GenerateJwtToken(existingUser);

            return Ok(new
            {
                Message = "Google login successful.",
                Token = token,
                User = new
                {
                    existingUser.Id,
                    existingUser.Username,
                    existingUser.Email
                }
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
    }
}

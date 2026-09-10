using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using WillCDev.Services;
using Shared.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net;
namespace WillCDev.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest request, IConfiguration configuration, AuthDbContext dbContext)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Username == request.Username);

            if (user != null && request.Password == user.PasswordHash)
            {
                // Implement JWT token generation logic here
                
                // Retrieve JWT settings from configuration
                var jwtConfig = configuration.GetSection("JwtSettings");
                var secretKey = jwtConfig["SecretKey"];
                var issuer = jwtConfig["Issuer"];
                var audience = jwtConfig["Audience"];
                var expiration = DateTime.UtcNow.AddHours(1);

                // Define the claims that blazor will read
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, request.Username),
                };

                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = expiration,
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = creds
                };
                
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new LoginResult { Token = tokenString });
            }
            else
            {
                return Unauthorized(new LoginResult { ErrorMessage = "Invalid username or password" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register([FromBody] RegisterRequest request, IConfiguration configuration, AuthDbContext dbContext)
        {
            var existingUser = dbContext.Users.FirstOrDefault(u => u.Username == request.Username);
            if (existingUser != null)
            {
                return Conflict(new RegisterResult { ErrorMessage = "Username already exists" });
            }

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                PasswordHash = request.Password
            };
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();

            return Ok(new RegisterResult { Success = true });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Users(AuthDbContext dbContext)
        {
            var users = dbContext.Users.Select(u => new { u.Id, u.Username }).ToList();
            return Ok(users);
        }
        
    }
}
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using WillCDev.Services;
namespace WillCDev.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request, IConfiguration configuration)
        {
            if (request.Username.Equals("admin", StringComparison.OrdinalIgnoreCase) && request.Password == "password")
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
    }
}
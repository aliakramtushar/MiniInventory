using Inventory.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Inventory.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // 🔐 In-memory refresh token store (for demo)
        private static readonly Dictionary<string, string> _refreshTokens = new();

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            if (login.Username == "admin" && login.Password == "admin123")
            {
                var token = GenerateJwtToken(login.Username);
                var refreshToken = Guid.NewGuid().ToString();

                // Store refresh token mapped to the user
                _refreshTokens[refreshToken] = login.Username;

                return Ok(new
                {
                    Token = token,
                    RefreshToken = refreshToken
                });
            }

            return Unauthorized("Invalid credentials.");
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] RefreshTokenDto model)
        {
            if (_refreshTokens.TryGetValue(model.RefreshToken, out var username))
            {
                var newToken = GenerateJwtToken(username);
                var newRefreshToken = Guid.NewGuid().ToString();

                // Replace old token with new one
                _refreshTokens.Remove(model.RefreshToken);
                _refreshTokens[newRefreshToken] = username;

                return Ok(new
                {
                    Token = newToken,
                    RefreshToken = newRefreshToken
                });
            }

            return Unauthorized("Invalid or expired refresh token.");
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] RefreshTokenDto model)
        {
            if (_refreshTokens.ContainsKey(model.RefreshToken))
            {
                _refreshTokens.Remove(model.RefreshToken);
                return Ok("Logged out successfully.");
            }

            return BadRequest("Invalid refresh token.");
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ViL.Api.Models;
using ViL.Common.Enums;
using ViL.Data.Models;
using ViL.Services.Services;

namespace ViL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {

        public IUsersService _usersService;
        private readonly IConfiguration Configuration;

        public LoginController(IUsersService usersService, IConfiguration configuration)
        {
            _usersService = usersService;
            Configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginInfo loginInfo)
        {
            try
            {
                var user = _usersService.Login(loginInfo.username, loginInfo.password);
                var token = GenerateJwtToken(user);
                return Ok(token);
            } catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        private string GenerateJwtToken(Users user)
        {
            var claims = new[]
            {
                new Claim("sub", user.UserId),
                new Claim("username", user.Username),
                new Claim("password", user.Password),
                new Claim("role", ((UserRole)user.Role).ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Configuration["JwtIssuer"],
                audience: Configuration["JwtAudience"],
                claims: claims,
                // Thời gian hết hạn của token
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            if (user.Role != 0)
            {
                token = new JwtSecurityToken(
                    issuer: Configuration["JwtIssuer"],
                audience: Configuration["JwtAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                // Thời gian hết hạn của token
                signingCredentials: creds
                );
            }

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

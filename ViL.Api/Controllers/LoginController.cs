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
                return Ok(new { token });
            } catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        private string GenerateJwtToken(Users user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId),
                new Claim(JwtRegisteredClaimNames.Name, user.DisplayName ?? user.Username),
                new Claim("username", user.Username),
                new Claim(ClaimTypes.Role, ((UserRole)user.Role).ToString()),                
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"] ?? "Secret Key"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = Configuration["Jwt:Issuer"],
                Audience = Configuration["Jwt:Audience"],
                Expires = user.Role == (int)UserRole.Admin ? DateTime.Now.AddHours(8) : DateTime.Now.AddDays(30),
                SigningCredentials = creds,
                Subject = new ClaimsIdentity(claims)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

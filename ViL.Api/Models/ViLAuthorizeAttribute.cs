using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ViL.Api.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ViLAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public  string? Role {get; set;}
        public  string? UserId { get; set;}

        public ViLAuthorizeAttribute()
        {
            Role = null;
            UserId = null;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context != null)
            {
                Microsoft.Extensions.Primitives.StringValues authToken;
                Microsoft.Extensions.Primitives.StringValues ownerId;
                context.HttpContext.Request.Headers.TryGetValue("access_token", out authToken);
                context.HttpContext.Request.Headers.TryGetValue("ownerId", out ownerId);

                var _token = authToken.FirstOrDefault();
                var _owner = ownerId.FirstOrDefault();
                if (_token == null)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
                try
                {
                    string token = _token.ToString();
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes("This is my ViLit Secret key for authentication");
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out var validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var role = jwtToken.Claims.First(c => c.Type == "role").Value;
                    if (!Role.IsNullOrEmpty() && role != Role)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                    
                    var userId = jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
                    if (_owner != null)
                    {
                        var owner = _owner.ToString();
                        if (userId != owner && role != "Admin")
                        {
                            context.Result = new UnauthorizedResult();
                            return;
                        }
                    }
                    context.HttpContext.Items["UserId"] = userId;
                }
                catch (Exception)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }

        public bool IsValidToken(string token)
        {
            return true;
        }
    }
}

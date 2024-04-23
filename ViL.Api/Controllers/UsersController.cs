using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using ViL.Api.Models;
using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Services.Services;

namespace ViL.Api.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("all")]
        [ViLAuthorize]        
        public IActionResult GetAll()
        {
            try
            {
                var query = _usersService.GetAll();
                var result = new List<UserInfo>();
                foreach (var item in query)
                {
                    result.Add(new UserInfo(item));
                }
                return Ok(result);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                var user = _usersService.GetById(id);
                if (user != null)
                {
                    return Ok(new UserInfo(user));
                } else
                {
                    return NoContent();
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        public IActionResult Search(string username = "", string email = "")
        {
            try
            {
                if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email))
                {
                    return BadRequest();
                }
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email))
                {
                    var query = _usersService.Get(u =>  u.Username == username && u.Email == email);
                    if (query != null)
                    {
                        return Ok(query.ToList());
                    } else
                    {
                        return StatusCode(204);
                    }
                } else 
                {
                    var query = _usersService.Get(u => u.Username == username || u.Email == email);
                    if (query != null)
                    {
                        return Ok(query.ToList());
                    } else
                    {
                        return StatusCode(204);
                    }
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        public IActionResult Add(Users user)
        {
            try
            {
                _usersService.Add(user);
                return StatusCode(201);
            } catch (Exception ex)
            {
                var error = new
                {
                    Msg = ex.Message,
                    ErrorMsgs = ex.Data["validateErr"]
                };
                return BadRequest(error);
            }            
        }

        [HttpPut("update")]
        [ViLAuthorize]
        public IActionResult Update(UserInfo userInfo)
        {
            try
            {                
                var user = _usersService.GetById(userInfo.UserId);
                if (user != null)
                {
                    _usersService.Update(user);
                    return Ok();
                } else
                {
                    return StatusCode(404);
                }                
            } catch (Exception ex)
            {
                var error = new
                {
                    Msg = ex.Message,
                    ErrorMsgs = ex.Data["validateErr"]
                };
                return BadRequest(error);
            }
        }

        [HttpDelete("delete")]
        public IActionResult Delete(string id)
        {
            try
            {
                _usersService.Delete(s => s.UserId == id);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut("change-password")]
        [ViLAuthorize]
        public IActionResult ChangePassword(PasswordModifier modifier)
        {
            try
            {
                if (modifier == null
                || modifier.UserId.IsNullOrEmpty()
                || modifier.OldPassword.IsNullOrEmpty()
                || modifier.NewPassword.IsNullOrEmpty())
                {
                    return BadRequest();
                }
                var user = _usersService.GetById(modifier.UserId);
                if (user != null && user.Password == modifier.OldPassword)
                {
                    user.Password = modifier.NewPassword;
                    _usersService.Update(user);
                }
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }
    }
}

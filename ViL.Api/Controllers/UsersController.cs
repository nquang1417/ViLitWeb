using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using ViL.Api.Models;
using ViL.Common.Enums;
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

        [HttpGet("get-users")]
        [ViLAuthorize(Role = "Admin")]
        public IActionResult Get(int page = 1)
        {
            try
            {
                var pageSize = 20;
                var query = _usersService.Get(user => user.Username != "viladmin").OrderBy(user => user.Username);
                if (query == null || query.Count() == 0)
                {
                    return StatusCode(204);
                }
                var queryUser = query.Skip(pageSize * (--page)).Take(pageSize);
                var users = new List<UserInfo>();
                foreach (var item in queryUser)
                {
                    users.Add(new UserInfo(item));
                }
                var result = new
                {
                    Data = users,
                    Totals = query.Count()
                };

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
                var newUser = new Users(user.Username, user.Password);
                newUser.Email = user.Email ?? "";
                newUser.DisplayName = user.DisplayName == null || user.DisplayName == string.Empty ? user.Username : user.DisplayName;
                _usersService.Add(newUser);
                return StatusCode(201, newUser);
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
                foreach (var prop in userInfo.GetType().GetProperties())
                {
                    var value = prop.GetValue(userInfo);
                    user.GetType().GetProperty(prop.Name)?.SetValue(user, value);
                }
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
        [ViLAuthorize]
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

        [HttpPut("change-status")]
        [ViLAuthorize]
        public IActionResult ChangeStatus(string userid, int status)
        {
            try
            {
                var query = _usersService.GetById(userid);
                if (query != null)
                {
                    query.Status = status;
                    _usersService.Update(query);
                    return Ok();
                } else
                {
                    return BadRequest();
                }
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
                } else if (user != null && user.Password != modifier.OldPassword)
                {
                    return StatusCode(401, "Mật khẩu không đúng!");
                }
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPut("ban-user")]
        [ViLAuthorize(Role = "Admin")]
        public IActionResult BanUser(string userId, BanedInfo baned)
        {
            try
            {
                var query = _usersService.GetById(userId);
                if (query != null)
                {
                    query.Status = (int)UserStatus.Banned;
                    query.BannedExpired = DateTime.Now.AddDays(baned.Duration);
                    query.About = baned.Reason;
                    _usersService.Update(query);
                    BackgroundJob.Schedule(() => _usersService.UnlockUser(userId), TimeSpan.FromDays(baned.Duration));
                }
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("unlock-user")]
        [ViLAuthorize(Role = "Admin")]
        public IActionResult UnlockUser(string userId)
        {
            try
            {
                var query = _usersService.GetById(userId);
                if (query != null)
                {
                    if (query.Status == (int)UserStatus.Banned)
                    {
                        _usersService.UnlockUser(userId);
                    }
                    return Ok();
                } else
                {
                    return StatusCode(404, "Người dùng không tồn tại");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

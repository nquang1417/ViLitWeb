using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                return Ok(_usersService.GetAll().ToList());
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
                    return Ok(user);
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
        public IActionResult Update(Users user)
        {
            try
            {                
                _usersService.Update(user);
                return Ok();
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
    }
}

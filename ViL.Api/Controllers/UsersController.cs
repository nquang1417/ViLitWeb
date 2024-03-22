using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
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

        [HttpGet] 
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_usersService.GetAll());
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
                return Ok(user);
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
                return Created();
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
            _usersService.Delete(s => s.UserId == id);
            return Ok();
        }
    }
}

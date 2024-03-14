using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Services.Services;

namespace ViL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private UsersService _usersService;
        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet] 
        public IActionResult GetAll()
        {
            return Ok(_usersService.GetAllUsers());
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(_usersService.GetById(id));
        }

        [HttpPost] 
        public IActionResult Add(Users user)
        {
            _usersService.AddUser(user);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            _usersService.Delete(s => s.UserId == id);
            return Ok();
        }
    }
}

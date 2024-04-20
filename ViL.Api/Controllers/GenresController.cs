using Microsoft.AspNetCore.Mvc;
using ViL.Data.Models;
using ViL.Services.Services;

namespace ViL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : Controller
    {
        private IGenresService _genresService;

        public GenresController(IGenresService genresService)
        {
            _genresService = genresService;
        }

        [HttpGet("lists")]
        public IActionResult GetAll()
        {
            try
            {
                var query = _genresService.GetAll();
                return Ok(query.ToList());
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("details")]
        public IActionResult Get(string genreId)
        {
            try
            {
                var query = _genresService.GetById(genreId);
                return Ok(query);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        public IActionResult Get(Genres genres)
        {
            try
            {
                var newGenre = new Genres(genres.GenreName);
                newGenre.CreateBy = genres.CreateBy;
                newGenre.UpdateBy = genres.UpdateBy;
                _genresService.Add(newGenre);
                return StatusCode(201);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edit")]
        public IActionResult Edit(Genres genres)
        {
            try
            {
                _genresService.Update(genres);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public IActionResult Delete(string genreId)
        {
            try
            {
                _genresService.Delete(genre => genre.GenreId == genreId);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

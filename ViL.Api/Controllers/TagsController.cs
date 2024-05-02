using Microsoft.AspNetCore.Mvc;
using ViL.Data.Models;
using ViL.Services.Services;

namespace ViL.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class TagsController : Controller
    {
        private ITagsService _tagsService;

        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }

        [HttpPost("add")]
        public IActionResult Add(Tags entity)
        {
            try
            {
                var query = _tagsService.Get(tag => tag.TagName.ToLower() == entity.TagName.ToLower()).FirstOrDefault();
                if (query == null)
                {
                    var newTag = new Tags(entity.TagName, entity.Description);
                    _tagsService.Add(newTag);
                    return StatusCode(201, newTag);
                } else
                {
                    return Ok();
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("multi-add")]
        public IActionResult MultiAdd(List<string> tags)
        {
            try
            {
                foreach (string tagName in tags)
                {
                    var query = _tagsService.Get(tag => tag.TagName.ToLower() == tagName.ToLower()).FirstOrDefault();
                    if (query == null)
                    {
                        var newTag = new Tags(tagName);
                        _tagsService.Add(newTag);
                    }
                }
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get")]
        public IActionResult Get(string id)
        {
            try
            {
                var query = _tagsService.GetById(id);
                if (query == null)
                {
                    return StatusCode(204);
                } else
                {
                    return Ok(query);
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ViL.Api.Models;
using ViL.Data.Models;
using ViL.Services.Services;

namespace ViL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookmarksController : Controller
    {
        private IBookmarksService _bookmarksService;

        public BookmarksController(IBookmarksService bookmarksService)
        {
            _bookmarksService = bookmarksService;
        }

        [HttpPost("new-bookmark")]
        [ViLAuthorize]
        public IActionResult AddBookmark(string userId, string chapterId)
        {
            try
            {
                var newBookmark = new Bookmarks(userId, chapterId);
                _bookmarksService.Add(newBookmark);
                return Ok(newBookmark);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
            
        }

        [HttpGet("all")]
        [ViLAuthorize]
        public IActionResult getAll(string userId)
        {
            try
            {
                var query = _bookmarksService.Get(bm => bm.UserId == userId);
                return Ok(query.ToList());
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("delete")]
        [ViLAuthorize]
        public IActionResult DeleteBookmark(string bookmarkId)
        {
            try
            {
                var bookmark = _bookmarksService.GetById(bookmarkId);
                if (bookmark != null)
                {
                    _bookmarksService.Delete(bookmark);
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
       
    }
}

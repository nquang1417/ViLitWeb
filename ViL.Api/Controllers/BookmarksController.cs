using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using ViL.Api.Models;
using ViL.Common.Commons;
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
        public IActionResult AddBookmark(Bookmarks entity)
        {
            try
            {
                var newBookmark = new Bookmarks(entity.UserId, entity.BookId, entity.ChapterId);
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

        [HttpGet("filter")]
        [ViLAuthorize]
        public IActionResult filter(FilterBookmark filter, int page = 1)
        {
            try
            {
                int pageSize = 20;
                Expression<Func<Bookmarks, bool>> filterExp = bm => true;
                if (filter.UserId != null && filter.UserId != "")
                {
                    Expression<Func<Bookmarks, bool>> filterUserid = bm => bm.UserId == filter.UserId;
                    VilHelpers.AndAlso(filterExp, filterUserid);
                }
                if (filter.BookId != null && filter.BookId != "")
                {
                    Expression<Func<Bookmarks, bool>> filterBook = bm => bm.BookId == filter.BookId;
                    VilHelpers.AndAlso(filterExp, filterBook);
                }

                var query = _bookmarksService.Get(filterExp);
                if (query != null)
                {
                    var result = new
                    {
                        Data = query.Skip(pageSize * (--page)).Take(pageSize).ToList(),
                        Totals = query.Count()
                    };
                    return Ok(result);
                } else
                {
                    return StatusCode(204);
                }
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

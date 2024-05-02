using Microsoft.AspNetCore.Mvc;
using ViL.Api.Models;
using ViL.Data.Models;
using ViL.Services.Services;

namespace ViL.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CommentsController : Controller
    {
        private IChapterCommentsService _chapterCommentsService;

        public CommentsController(IChapterCommentsService chapterCommentsService)
        {
            _chapterCommentsService = chapterCommentsService;
        }

        [HttpGet("filterComments")]
        public IActionResult FilterComments(string chapterId, int page = 1)
        {
            try
            {
                var pageSize = 20;
                var query = _chapterCommentsService.Get(cmt => cmt.ChapterId == chapterId)
                                                   .OrderByDescending(c => c.CreateDate);
                                                   
                if (query == null)
                {
                    return StatusCode(204);
                }
                var result = new
                {
                    Data = query.Skip(pageSize * (--page)).Take(pageSize),
                    Totals = query.Count()
                };
                return Ok(result);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        [ViLAuthorize]
        public IActionResult Comment(ChapterComments comment)
        {
            try
            {
                comment.CommentId = Guid.NewGuid().ToString();
                _chapterCommentsService.Add(comment);
                return StatusCode(201, comment.CommentId);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("edit")]
        [ViLAuthorize] 
        public IActionResult EditComment(ChapterComments comment)
        {
            try
            {
                var query = _chapterCommentsService.GetById(comment.CommentId);
                if (query == null)
                {
                    return StatusCode(404);

                }
                comment.UpdateDate = DateTime.Now;
                _chapterCommentsService.Update(comment);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        [ViLAuthorize]
        public IActionResult DeleteComment(string commentId)
        {
            try
            {
                var query = _chapterCommentsService.GetById(commentId);
                if (query == null)
                {
                    return StatusCode(404);
                }
                _chapterCommentsService.Delete(query);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

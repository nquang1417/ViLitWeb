using Microsoft.AspNetCore.Mvc;
using ViL.Api.Models;
using ViL.Services.Services;

namespace ViL.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ReactionsController : Controller
    {
        private IUserFavoriteBooksService _favoriteBooksService;
        private IUserLikedChaptersService _likedChaptersService;
        private IUserLikedCommentsService _likedCommentsService;
        private IUserLikedReviewsService _likedReviewsService;

        public ReactionsController(IUserFavoriteBooksService favoriteBooksService,
                                   IUserLikedChaptersService likedChaptersService,
                                   IUserLikedCommentsService likedCommentsService,
                                   IUserLikedReviewsService likedReviewsService)
        {
            _favoriteBooksService = favoriteBooksService;
            _likedChaptersService = likedChaptersService;
            _likedCommentsService = likedCommentsService;
            _likedReviewsService = likedReviewsService;
        }

        [HttpGet("followsStatus")]
        [ViLAuthorize]
        public IActionResult GetFollowBookStatus(string userId, string bookId)
        {
            try
            {
                var query = _favoriteBooksService.Get(followStatus => followStatus.BookId == bookId && followStatus.UserId == userId).FirstOrDefault();
                if (query == null)
                {
                    return Ok(false);
                }
                if (query.Status == 1)
                {
                    return Ok(true);
                } else
                {
                    return Ok(false);
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("chapterReaction")]
        [ViLAuthorize]
        public IActionResult GetChapterReaction(string userId, string chapterId)
        {
            try
            {
                var query = _likedChaptersService.Get(reaction => reaction.UserId == userId && reaction.ChapterId == chapterId).FirstOrDefault();
                if (query == null)
                {
                    return Ok(false);
                } 
                if (query.Status == 1)
                {
                    return Ok(true);
                } else
                {
                    return Ok(false);
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("commentReaction")]
        [ViLAuthorize]
        public IActionResult GetCommentReaction(string userId, string commentId)
        {
            try
            {
                var query = _likedCommentsService.Get(reaction => reaction.UserId == userId && reaction.CommentId == commentId).FirstOrDefault();
                if (query == null)
                {
                    return Ok(false);
                }
                if (query.Status == 1)
                {
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("reviewReaction")]
        [ViLAuthorize]
        public IActionResult GetReviewReaction(string userId, string reviewId)
        {
            try
            {
                var query = _likedReviewsService.Get(reaction => reaction.UserId == userId && reaction.ReviewId == reviewId).FirstOrDefault();
                if (query == null)
                {
                    return Ok(false);
                }
                if (query.Status == 1)
                {
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}

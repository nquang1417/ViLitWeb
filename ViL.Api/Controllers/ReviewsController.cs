using Microsoft.AspNetCore.Mvc;
using ViL.Api.Models;
using ViL.Data.Models;
using ViL.Services.Services;

namespace ViL.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ReviewsController : Controller
    {
        private IBookReviewsService _reviewsService;
        private IUsersService _usersService;

        public ReviewsController(IBookReviewsService reviewsService, IUsersService usersService)
        {
            _reviewsService = reviewsService;
            _usersService = usersService;
        }


        [HttpGet("filter")]
        public IActionResult FilterReviews(string bookId, int page = 1)
        {
            try
            {
                var pageSize = 20;
                var reviews = _reviewsService.Get(review => review.BookId == bookId).OrderByDescending(c => c.CreateDate);
                var users = _usersService.GetAll();
                var query = from review in reviews
                            join user in users on review.UserId equals user.UserId
                            select new { 
                                review.ReviewId, review.BookId, review.UserId,
                                user.DisplayName, user.Avatar, review.ReviewContent,
                                review.RatingScore, review.Likes, review.Replies, review.ParentReviewId,
                                review.Status, review.CreateDate, review.CreateBy,
                                review.UpdateDate, review.UpdateBy
                            };

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

        [HttpPost("add")]
        [ViLAuthorize]
        public IActionResult Add(BookReviews review)
        {
            try
            {
                var newReview = new BookReviews(review.BookId, review.UserId, review.ReviewContent, review.RatingScore);
                review.ReviewId = Guid.NewGuid().ToString();
                newReview.CreateBy = review.UserId;
                newReview.UpdateBy = review.UserId;
                _reviewsService.Add(newReview);
                return StatusCode(201, newReview);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edit")]
        [ViLAuthorize]
        public IActionResult Edit(BookReviews review)
        {
            try
            {
                var query = _reviewsService.GetById(review.ReviewId);
                if (query == null )
                {
                    return StatusCode(404);
                }
                review.UpdateDate = DateTime.Now;
                _reviewsService.Update(review);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        [ViLAuthorize]
        public IActionResult Delete(string reviewId)
        {
            try
            {
                var query = _reviewsService.GetById(reviewId);
                if (query == null )
                {
                    return StatusCode(404);
                }
                _reviewsService.Delete(query);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

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

        public ReviewsController(IBookReviewsService reviewsService)
        {
            _reviewsService = reviewsService;
        }


        [HttpGet("filter")]
        public IActionResult FilterReviews(string bookId, int page = 1)
        {
            try
            {
                var pageSize = 20;
                var query = _reviewsService.Get(review => review.BookId == bookId).OrderByDescending(c => c.CreateDate);
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
                review.ReviewId = Guid.NewGuid().ToString();
                _reviewsService.Add(review);
                return StatusCode(201, review.ReviewId);
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

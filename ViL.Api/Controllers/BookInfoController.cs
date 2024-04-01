using Microsoft.AspNetCore.Mvc;
using ViL.Services.Services;
using ViL.Common.Commons;
using ViL.Data.Models;

namespace ViL.Api.Controllers
{
    public class BookInfoController : Controller
    {
        private IBookInfoService _bookInfoService;

        public BookInfoController(IBookInfoService bookInfoService)
        {
            _bookInfoService = bookInfoService;
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            try
            {
                var query = _bookInfoService.GetAllDetails();
                return Ok(query);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("details")]
        public IActionResult GetBookDetails(string bookId)
        {
            try
            {
                var query = _bookInfoService.GetBookDetails(bookId);
                if (query == null)
                {
                    return NoContent();
                }
                return Ok(query);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery]string title)
        {
            try
            {
                var query = _bookInfoService.Get(book => book.BookTitle.Contains(title));
                return Ok(query);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("add")] 
        public IActionResult Add(BookInfo bookInfo)
        {
            try
            {
                _bookInfoService.Add(bookInfo);
                return Created();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public IActionResult Update(string bookId, [FromBody]BookInfo bookInfo)
        {
            try
            {
                var query = _bookInfoService.GetById(bookId);
                if (query != null)
                {
                    _bookInfoService.Update(bookInfo);
                }
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public IActionResult Delete(string bookId)
        {
            try
            {
                var query = _bookInfoService.GetById(bookId);
                if (query != null)
                {
                    _bookInfoService.Delete(b => b.BookId == bookId);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

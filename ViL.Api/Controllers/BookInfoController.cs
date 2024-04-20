using Microsoft.AspNetCore.Mvc;
using ViL.Services.Services;
using ViL.Data.Models;
using ViL.Api.Models;
using System.Linq.Expressions;
using ViL.Common.Commons;

namespace ViL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookInfoController : Controller
    {
        private IBookInfoService _bookInfoService;
        private IWebHostEnvironment _webHostEnvironment;

        public BookInfoController(IBookInfoService bookInfoService, IWebHostEnvironment webHostEnvironment)
        {
            _bookInfoService = bookInfoService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            try
            {
                var query = _bookInfoService.GetAllDetails();
                if (query == null)
                {
                    return StatusCode(204);
                }
                else
                {
                    return Ok(query);
                }
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
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        public IActionResult Search(string title = "")
        {
            try
            {
                var query = _bookInfoService.Get(book => book.BookTitle.StartsWith(title));
                if (query != null)
                {
                    var summary = from book in query
                                  select new
                                  {
                                      book.BookId,
                                      book.BookTitle,
                                      book.AuthorName,
                                  };
                }
                if (query != null)
                {
                    return Ok(query.ToList());
                } else
                {
                    return StatusCode(204);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("filter")]
        public IActionResult FilterBooks(FilterBook filter, int page = 1)
        {
            try
            {
                var pageSize = 20;
                Expression<Func<BookInfo, bool>> condition = book => true;

                // Append Condition Linq Expression
                if (filter.Title != null && filter.Title != string.Empty)
                {
                    Expression<Func<BookInfo, bool>> title = book => book.BookTitle.StartsWith(filter.Title);     
                    condition = VilHelpers.AndAlso(condition, title);
                }
                if (filter.AuthorName != null && filter.AuthorName != string.Empty)
                {
                    Expression<Func<BookInfo, bool>> author = book => book.AuthorName == filter.AuthorName;
                    condition = VilHelpers.AndAlso(condition, author);
                }
                if (filter.GenreId != null && filter.GenreId != string.Empty)
                {
                    Expression<Func<BookInfo, bool>> genre = book => book.GenreId == filter.GenreId;  
                    condition = VilHelpers.AndAlso(condition, genre);
                }
                if (filter.Status != null)
                {
                    Expression<Func<BookInfo, bool>> statusFilter = book => book.Status == filter.Status;
                    condition = VilHelpers.AndAlso(condition, statusFilter);
                }
                if (filter.UploaderId != null && filter.UploaderId != string.Empty)
                {
                    Expression<Func<BookInfo, bool>> updloaderFilter = book => book.UploaderId == filter.UploaderId;
                    condition = VilHelpers.AndAlso(condition, updloaderFilter);
                }

                var query = _bookInfoService.Get(condition).Skip(pageSize * (--page)).Take(pageSize);
                if (query != null)
                {
                    return Ok(query.ToList());
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
        public IActionResult Add(BookInfo bookInfo)
        {
            try
            {
                var newBook = new BookInfo(bookInfo.BookTitle, bookInfo.GenreId, bookInfo.UploaderId);
                newBook.AuthorName = bookInfo.AuthorName ?? newBook.AuthorName;
                newBook.Description = bookInfo.Description ?? newBook.AuthorName;
                newBook.BookCover = bookInfo.BookCover ?? newBook.AuthorName;
                string folderPath = newBook.Url ?? $"..\\Data\\{newBook.BookId}";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                _bookInfoService.Add(newBook);
                return StatusCode(201, newBook);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upload-cover")]
        [ViLAuthorize]
        public async Task<IActionResult> UploadCover([FromForm] IFormFile file)
        {
            try
            {
                var apiKey = "4dc088e5e644f3290818694864da17d0";
                var apiUrl = $"https://api.imgbb.com/1/upload?key={apiKey}";
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Invalid file to upload!");
                }
                using (HttpClient client = new HttpClient())
                {
                    using (var uploadStream = new MemoryStream())
                    {
                        await file.CopyToAsync(uploadStream);
                        var formData = new MultipartFormDataContent();

                        formData.Add(new StringContent(apiKey), "key");
                        formData.Add(new ByteArrayContent(uploadStream.ToArray()), "image", file.FileName);
                     

                        var response = await client.PostAsync(apiUrl, formData);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            return Ok(result);
                        } else
                        {
                            return BadRequest();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        [ViLAuthorize]
        public IActionResult Edit(string bookId, [FromBody]BookInfo bookInfo)
        {
            try
            {
                _bookInfoService.Update(bookInfo);

                /*var query = _bookInfoService.GetById(bookId);
                if (query != null)
                {
                    _bookInfoService.Update(bookInfo);
                }*/
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        [ViLAuthorize]
        public IActionResult Delete(string bookId)
        {
            try
            {
                var query = _bookInfoService.GetById(bookId);
                if (query != null)
                {
                    _bookInfoService.Delete(query);
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

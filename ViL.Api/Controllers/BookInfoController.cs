using Microsoft.AspNetCore.Mvc;
using ViL.Services.Services;
using ViL.Data.Models;
using ViL.Api.Models;
using System.Linq.Expressions;
using ViL.Common.Commons;
using ViL.Common.Enums;
using Hangfire;

namespace ViL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookInfoController : Controller
    {
        private IBookInfoService _bookInfoService;
        private IUserFavoriteBooksService _userFavoriteBooksService;
        private IBookStatisticsInfoService _bookStatisticsInfoService;
        private INotificationsService _notificationsService;


        public BookInfoController(IBookInfoService bookInfoService,
                                  IUserFavoriteBooksService userFavoriteBooksService,
                                  IBookStatisticsInfoService bookStatisticsInfoService,
                                  INotificationsService notificationsService)
        {
            _bookInfoService = bookInfoService;
            _userFavoriteBooksService = userFavoriteBooksService;
            _bookStatisticsInfoService = bookStatisticsInfoService;
            _notificationsService = notificationsService;
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

        [HttpGet("new-updates")]
        public IActionResult GetNewUpdates()
        {
            try
            {
                var query = _bookInfoService.GetAllDetails();
                if (query == null)
                {
                    return StatusCode(204);
                } else
                {
                    var result = query.AsEnumerable().OrderByDescending(book => book.UpdateDate).Take(20);
                    return Ok(result);
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("book-stats")]
        public IActionResult GetBookStats(string bookId)
        {
            try
            {
                var query = _bookStatisticsInfoService.Get(book => book.BookId == bookId).FirstOrDefault();
                if (query == null)
                {
                    return NoContent();
                }
                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("hot-books")]
        public IActionResult GetHotBooks()
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
                    var result = query.AsEnumerable().OrderByDescending(book => book.RankScore).Take(20);
                    return Ok(result);
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
                    var statusConditions = new List<Expression<Func<BookInfo, bool>>>();

                    foreach (var item in filter.Status)
                    {
                        Expression<Func<BookInfo, bool>> statusFilter = book => book.Status == item;
                        statusConditions.Add(statusFilter);
                    }
                    var statusCondition = statusConditions.Aggregate((acc, next) => VilHelpers.OrElse(acc, next));
                    condition = VilHelpers.AndAlso(condition, statusCondition);
                }
                if (filter.UploaderId != null && filter.UploaderId != string.Empty)
                {
                    Expression<Func<BookInfo, bool>> updloaderFilter = book => book.UploaderId == filter.UploaderId;
                    condition = VilHelpers.AndAlso(condition, updloaderFilter);
                }

                var query = _bookInfoService.Get(condition);
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
                var newNoti = new Notifications(1, newBook.UploaderId, $"Bạn đã tạo truyện mới với tiêu đề {newBook.BookTitle}");
                _notificationsService.Add(newNoti);
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

        [HttpPut("update-stats")]
        public IActionResult UpdateStats(BookStatisticsInfo statsInfo)
        {
            try
            {
                var query = _bookStatisticsInfoService.Get(stats => stats.BookId == statsInfo.BookId).FirstOrDefault();
                if (query == null)
                {
                    return BadRequest();
                }
                else
                {
                    query.Views = statsInfo.Views;
                    query.Followers = statsInfo.Followers;
                    query.Comments = statsInfo.Comments;
                    query.Reviews = statsInfo.Reviews;
                    query.AverageRating = statsInfo.AverageRating;
                    query.UpdateDate = DateTime.Now;
                }
                _bookStatisticsInfoService.Update(query);
                return Ok();
            }
            catch (Exception ex)
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
                    if (Directory.Exists(query.Url))
                    {
                        Directory.Delete(query.Url, true);
                    }
                    _bookInfoService.Delete(query);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("follow-book")]
        [ViLAuthorize]
        public IActionResult Follow(string bookId, string userId)
        {
            try
            {
                var query = _userFavoriteBooksService.Get(record => record.BookId == bookId && record.UserId == userId).FirstOrDefault();
                if (query == null)
                {
                    var newRecord = new UserFavoriteBooks(bookId, userId);
                    _userFavoriteBooksService.Add(newRecord);                    
                } else
                {
                    switch (query.Status)
                    {
                        case 1:
                            query.Status = 0; break;
                        case 0:
                            query.Status = 1; break;
                    }
                    _userFavoriteBooksService.Update(query);
                }
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("has-bookmarks")]
        [ViLAuthorize]
        public IActionResult GetBooksHasBookmarks(string userId, int page = 1)
        {
            try
            {
                //var userId = HttpContext.Items["UserId"]?.ToString();
                var pageSize = 5;
                var query = _bookInfoService.GetBookHasBookmarks(userId);
                var result = new
                {
                    Data = query.Skip(pageSize * (--page)).Take(pageSize).ToList(),
                    Totals = query.Count()
                };
                return Ok(result);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-bookshelf")]
        [ViLAuthorize]
        public IActionResult GetFollowingBooks(string userId)
        {
            try
            {
                var query = _bookInfoService.GetFollowingBook(userId);
                if (query == null)
                {
                    return StatusCode(204);
                } else
                {
                    return Ok(query.ToList());
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("lock-book")]
        [ViLAuthorize(Role = "Admin")]
        public IActionResult LockBook(string bookId, BanedInfo locked)
        {
            try
            {
                var query = _bookInfoService.GetById(bookId);
                if (query != null)
                {
                    query.Status = (int)BookStatus.Locked;
                    query.LockedExpired = DateTime.Now.AddDays(locked.Duration);
                    query.LockedReason = locked.Reason;
                    _bookInfoService.Update(query);
                    BackgroundJob.Schedule(() => _bookInfoService.UnlockBook(bookId), TimeSpan.FromDays(locked.Duration));
                    return Ok(query.LockedExpired);
                } else
                {
                    return StatusCode(404, "Truyện không tồn tại");
                }
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("unlock-book")]
        [ViLAuthorize(Role = "Admin")]
        public IActionResult UnlockBook(string bookId)
        {
            try
            {
                var query = _bookInfoService.GetById(bookId);
                if (query != null)
                {
                    if (query.Status == (int)BookStatus.Locked)
                    {
                        _bookInfoService.UnlockBook(bookId);
                    }
                    return Ok();
                }
                else
                {
                    return StatusCode(404, "Truyện không tồn tại");
                }
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Hangfire;
using Microsoft.AspNetCore.Mvc;

using ViL.Api.Models;
using ViL.Common.Enums;
using ViL.Data.Models;
using ViL.Services.Services;

namespace ViL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookChaptersController : Controller
    {
        public IBookChaptersService _bookChaptersService;

        public BookChaptersController(IBookChaptersService bookChaptersService)
        {
            _bookChaptersService = bookChaptersService;
        }

        [HttpGet("all-summary")]
        public IActionResult GetAll(string bookId)
        {
            try
            {
                var query = _bookChaptersService.Get(chapter => chapter.BookId == bookId
                                                           && chapter.Status == (int)ChapterStatus.Published)
                                                .OrderBy(chapter => chapter.ChapterNum);
                if (query == null || query.Count() == 0)
                {
                    return StatusCode(204);
                } else
                {
                    var result = from chapter in query
                                 select new
                                 {
                                     chapter.ChapterId,
                                     chapter.ChapterNum,
                                     chapter.ChapterTitle,
                                 };
                    return Ok(result.ToList());
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all-publisheds")] 
        public IActionResult GetAllPublisheds(string bookId)
        {
            try
            {
                var query = _bookChaptersService.Get(chapter => chapter.BookId == bookId
                                                           && chapter.Status == (int)ChapterStatus.Published)
                                                .OrderBy(chapter => chapter.ChapterNum);
                if (query == null || query.Count() == 0)
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

        [HttpGet("get-chapters")]
        public IActionResult GetChapters(string bookId, int page)
        {
            try
            {
                var pageSize = 20;
                var query = _bookChaptersService.Get(chapter => chapter.BookId == bookId
                                                           && chapter.Status == (int)ChapterStatus.Published)
                                                .OrderBy(chapter => chapter.ChapterNum);
                
                if (query == null || query.Count() == 0)
                {
                    return StatusCode(204);
                }
                else
                {
                    var result = new
                    {
                        Data = query.Skip(pageSize * (--page)).Take(pageSize).ToList(),
                        Totals = query.Count()
                    };
                    return Ok(result);
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } 

        [HttpGet("all-drafts")]
        [ViLAuthorize]
        public IActionResult GetAllDrafts(string bookId)
        {
            try
            {
                var query = _bookChaptersService.Get(chapter => chapter.BookId == bookId
                                                           && chapter.Status == (int)ChapterStatus.Draft)
                                                .OrderBy(chapter => chapter.ChapterNum);
                if (query == null || query.Count() == 0)
                {
                    return StatusCode(204);
                }
                else
                {
                    return Ok(query.ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-drafts")]
        [ViLAuthorize]
        public IActionResult GetDrafts(string bookId, int page = 1)
        {
            try
            {
                var pageSize = 20;
                var query = _bookChaptersService.Get(chapter => chapter.BookId == bookId
                                                           && chapter.Status == (int)ChapterStatus.Draft)
                                                .OrderBy(chapter => chapter.ChapterNum);                                                
                if (query == null || query.Count() == 0)
                {
                    return StatusCode(204);
                }
                else
                {
                    var result = new
                    {
                        Data = query.Skip(pageSize * (--page)).Take(pageSize).ToList(),
                        Totals = query.Count()
                    };
                    return Ok(result);
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all-deleteds")]
        [ViLAuthorize]
        public IActionResult GetAllDeleteds(string bookId)
        {
            try
            {
                var query = _bookChaptersService.Get(chapter => chapter.BookId == bookId
                                                           && chapter.Status == (int)ChapterStatus.Deleted)
                                                .OrderBy(chapter => chapter.ChapterNum);
                if (query == null || query.Count() == 0)
                {
                    return StatusCode(204);
                }
                else
                {
                    return Ok(query.ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-deleteds")]
        [ViLAuthorize]
        public IActionResult GetDeleteds(string bookId, int page)
        {
            try
            {
                var pageSize = 20;
                var query = _bookChaptersService.Get(chapter => chapter.BookId == bookId
                                                           && chapter.Status == (int)ChapterStatus.Deleted)
                                                .OrderBy(chapter => chapter.ChapterNum);
                if (query == null || query.Count() == 0)
                {
                    return StatusCode(204);
                }
                else
                {
                    var result = new
                    {
                        Data = query.Skip(pageSize * (--page)).Take(pageSize).ToList(),
                        Totals = query.Count()
                    };
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-chapter")] 
        public IActionResult Get(string chapterId)
        {
            try
            {
                var query = _bookChaptersService.GetById(chapterId);
                if (query == null)
                {
                    return StatusCode(204);
                } else
                {
                    string path = query.FileName ?? string.Empty;
                    var _content = System.IO.File.ReadAllBytes(path);
                    //byte[] bytes = Encoding.UTF8.GetBytes(_content);
                    //_content = Convert.ToBase64String(bytes);
                    var file = File(_content, "text/plain");
                    return Ok(new {Chapter = query, File = file});
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("new-chapter")]
        [ViLAuthorize]
        public IActionResult GenerateNewChapter(string bookId)
        {
            try
            {
                var query = _bookChaptersService.Get(chapter => chapter.BookId == bookId)
                                                .OrderByDescending(chapter => chapter.ChapterNum)
                                                .First();
                var uploader = HttpContext.Items["UserId"]?.ToString();
                var newChapNo = ++query.ChapterNum;
                var newChapter = new BookChapters(bookId, newChapNo, uploader);
                newChapter.Url = query.Url;
                return Ok(newChapter);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        [ViLAuthorize]
        public IActionResult Add(BookChapters entity)
        {
            try
            {
                var uploader = HttpContext.Items["UserId"]?.ToString();
                var newChapter = new BookChapters(entity.BookId, entity.ChapterNum, uploader, entity.ChapterTitle);
                _bookChaptersService.Add(newChapter);
                return Ok();

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upload-chapter")]
        [ViLAuthorize]
        public async Task<IActionResult> Upload(string chapterId, [FromForm] IFormFile file)
        {
            try
            {
                var newChapter = _bookChaptersService.GetById(chapterId);
                
                string folderPath = Path.GetDirectoryName(newChapter.FileName) ?? $"..\\Data\\{newChapter.BookId}\\text\\";
                string fileName = Path.GetFileName(newChapter.FileName) ?? $"{newChapter.ChapterNum.ToString()?.PadLeft(5, '0')}.txt";
                
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string filePath = newChapter.FileName ?? $"{folderPath}\\{newChapter.ChapterNum.ToString()?.PadLeft(5, '0')}.txt";
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                BackgroundJob.Enqueue(() => _bookChaptersService.ProcessAudioAsync(folderPath, fileName, newChapter.UploaderId));
                return StatusCode(201);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("unpublish")]
        [ViLAuthorize]
        public IActionResult UnPublish(string chapterId)
        {
            try
            {
                var query = _bookChaptersService.Get(chapter => chapter.ChapterId == chapterId
                                                           && chapter.Status == (int)ChapterStatus.Published).First();
                if (query == null)
                {
                    return NotFound();
                }
                else
                {
                    _bookChaptersService.ChangeStatus(query, (int)ChapterStatus.Draft);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("publish")]
        [ViLAuthorize]
        public IActionResult Publish(string chapterId)
        {
            try
            {
                var query = _bookChaptersService.Get(chapter => chapter.ChapterId == chapterId
                                                           && chapter.Status == (int)ChapterStatus.Draft).First();
                if (query == null)
                {
                    return NotFound();
                } else
                {
                    _bookChaptersService.ChangeStatus(query, (int)ChapterStatus.Published);
                    return Ok();
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }        

        [HttpPut("change-status")]
        [ViLAuthorize]
        public IActionResult ChangeStatus(string chapterId, int status)
        {
            try
            {
                var query = _bookChaptersService.GetById(chapterId);
                if (query == null)
                {
                    return NotFound();
                } else
                {
                    _bookChaptersService.ChangeStatus(query, status);
                    return Ok();
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        [ViLAuthorize]
        public IActionResult Delete(string chapterId)
        {
            try
            {
                var query = _bookChaptersService.GetById(chapterId);
                if (query == null)
                {
                    return NotFound();
                }
                else
                {
                    _bookChaptersService.Delete(query);
                    return Ok();
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("empty-trash")]
        [ViLAuthorize]
        public IActionResult EmptyTrash(string bookId)
        {
            try
            {
                _bookChaptersService.Delete(chapter => chapter.BookId == bookId
                                                       && chapter.Status == (int)ChapterStatus.Deleted);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

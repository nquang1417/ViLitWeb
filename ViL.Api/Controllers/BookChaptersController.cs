﻿using Hangfire;
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
        private IBookChaptersService _bookChaptersService;
        private IBookInfoService _bookInfoService;
        private INotificationsService _notificationsService;

        public BookChaptersController(IBookChaptersService bookChaptersService,
                                      IBookInfoService bookInfoService,
                                      INotificationsService notificationsService)
        {
            _bookChaptersService = bookChaptersService;
            _bookInfoService = bookInfoService;
            _notificationsService = notificationsService;
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
                    var book = _bookInfoService.GetById(query.BookId);
                    if (book != null && book.Status != (int)BookStatus.Locked)
                    {
                        string path = query.FileName ?? string.Empty;
                        var _content = System.IO.File.ReadAllBytes(path);
                        var file = File(_content, "text/plain");
                        return Ok(new { Chapter = query, File = file });
                    } else
                    {
                        if (book != null && book.Status == (int)BookStatus.Locked)
                        {
                            return StatusCode(404, new
                            {
                                Message = $"Truyện {book.BookTitle} hiện đang bị khóa",
                                Reason = book.LockedReason,
                                ExpiredDate = book.LockedExpired
                            });
                        } else
                        {
                            return StatusCode(404, "Truyện không tồn tại");
                        }
                    }
                    
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("edit-chapter")]
        [ViLAuthorize]
        public IActionResult Edit(string chapterId)
        {
            try
            {
                var query = _bookChaptersService.GetById(chapterId);

                if (query == null)
                {
                    return StatusCode(204);
                }
                else
                {
                    var book = _bookInfoService.GetById(query.BookId);
                    if (book != null)
                    {
                        string path = query.FileName ?? string.Empty;
                        var _content = System.IO.File.ReadAllBytes(path);
                        var file = File(_content, "text/plain");
                        return Ok(new { Chapter = query, File = file });
                    }
                    else
                    {
                        return StatusCode(404, "Truyện không tồn tại");
                    }

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-audio")]
        public IActionResult GetAudio(string chapterId)
        {
            try
            {
                var query = _bookChaptersService.GetById(chapterId);
                if (query == null)
                {
                    return StatusCode(204);
                }
                else
                {
                    string path = $"..\\Data\\{query.BookId}\\audio";
                    string filename = $"{query.ChapterNum.ToString()?.PadLeft(5, '0')}.wav";
                    var fileBytes = System.IO.File.ReadAllBytes(Path.Combine(path, filename));
                    return File(fileBytes, "audio/wav");
                }              
            } catch(Exception ex)
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
                var query = _bookChaptersService.Get(chapter => chapter.BookId == bookId && chapter.Status != (int)ChapterStatus.Deleted);
                BookChapters chapter = new BookChapters(bookId, 0);
                if (query != null)
                {
                    if (query.Count() > 0)
                    {
                        chapter = query.OrderByDescending(chapter => chapter.ChapterNum).First();
                    } 
                }
                                                
                var uploader = HttpContext.Items["UserId"]?.ToString();
                var newChapNo = ++chapter.ChapterNum;
                var newChapter = new BookChapters(bookId, newChapNo, uploader);
                newChapter.Url = chapter.Url;
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
                _bookChaptersService.Add(entity);
                var message = "";
                if (entity.Status == (int)ChapterStatus.Draft)
                {
                    message += $"Bản nháp chương {entity.ChapterTitle} đã được lưu";
                } 
                if (entity.Status == (int)ChapterStatus.Published)
                {
                    message += $"Chương {entity.ChapterTitle} đã được xuất bản";

                }
                var newNoti = new Notifications(1, entity.UploaderId, message);
                return StatusCode(201, entity);
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
                if (newChapter.Status == 1)
                {
                    BackgroundJob.Enqueue(() => _bookChaptersService.ProcessAudioAsync(folderPath, fileName, newChapter.UploaderId, $"Chương {newChapter.ChapterTitle} đã hoàn tất chuyển sang audio"));
                }
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
                    var message = "";
                    if (status == (int)ChapterStatus.Draft)
                    {
                        message += $"Bản nháp chương {query.ChapterTitle} đã được lưu";
                    }
                    if (status == (int)ChapterStatus.Published)
                    {
                        message += $"Chương {query.ChapterTitle} đã được xuất bản";

                    }
                    var newNoti = new Notifications(1, query.UploaderId, message);
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
                    var filePath = query.FileName;
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
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

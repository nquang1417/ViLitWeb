using Microsoft.AspNetCore.Mvc;
using ViL.Api.Models;
using ViL.Common.Enums;
using ViL.Services.Services;

namespace ViL.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class NotificationsController : Controller
    {
        private INotificationsService _notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        [HttpGet("getNoti")]
        [ViLAuthorize]
        public IActionResult GetNotifications(string userId)
        {
            try
            {
                var query = _notificationsService.Get(noti => noti.Recipient == userId);
                if (query == null)
                {
                    return StatusCode(404);
                }
                var countUnread = query.Count(noti => noti.Status == (int)NotiStatus.Unread);
                var result = new
                {
                    data = query.ToList(),
                    unreads = countUnread
                };
                return Ok(result);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("readAll")]
        [ViLAuthorize]
        public IActionResult MarkAsReadAll(string userId)
        {
            try
            {
                var query = _notificationsService.Get(noti => noti.Recipient == userId);
                if (query == null)
                {
                    return StatusCode(404);
                }
                foreach (var item in query)
                {
                    if (item.Status == (int)NotiStatus.Unread)
                    {
                        item.Status = (int)NotiStatus.Read;
                    }
                    _notificationsService.Update(item);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("markRead")]
        [ViLAuthorize]
        public IActionResult MarkAsRead(string notiId)
        {
            try
            {
                var query = _notificationsService.GetById(notiId);
                if (query == null)
                {
                    return StatusCode(404);
                }
                query.Status = (int)NotiStatus.Read;
                _notificationsService.Update(query);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        [ViLAuthorize]
        public IActionResult Delete(string notiId)
        {
            try
            {
                var query = _notificationsService.GetById(notiId);
                if (query == null)
                {
                    return StatusCode(404);
                }
                _notificationsService.Delete(noti => noti.NotiId == notiId);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using ViL.Api.Models;
using ViL.Common.Commons;
using ViL.Data.Models;
using ViL.Services.Services;

namespace ViL.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ReportsController : Controller
    {
        private IReportsService _reportsService;

        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpGet("all")]
        [ViLAuthorize(Role = "Admin")]
        public IActionResult GetAll()
        {
            try
            {
                var query = _reportsService.GetAll();
                return Ok(query);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("filter")]
        [ViLAuthorize(Role = "Admin")]
        public IActionResult FilterReports(FilterReports filter, int page = 1)
        {
            try
            {
                var pageSize = 20;
                Expression<Func<Reports, bool>> condition = report => true;

                if (filter.ReportedType != null)
                {
                    Expression<Func<Reports, bool>> type = report => report.ReportedType == filter.ReportedType;
                    condition = VilHelpers.AndAlso(condition, type);
                }
                if (filter.ReportEntityType != null)
                {
                    Expression<Func<Reports, bool>> entityType = report => report.TargetType == filter.ReportEntityType;
                    condition = VilHelpers.AndAlso(condition, entityType);
                }
                if (filter.Status != null)
                {
                    Expression<Func<Reports, bool>> status = report => report.Status == filter.Status;
                    condition = VilHelpers.AndAlso(condition, status);
                }
                var query = _reportsService.Get(condition);
                if (query != null)
                {
                    var result = new
                    {
                        Data = query.OrderByDescending(report => report.CreateDate).Skip(pageSize * (--page)).Take(pageSize).ToList(),
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

        [HttpPost("report")]
        public IActionResult Add(Reports report)
        {
            try
            {
                if (report == null)
                {
                    return BadRequest("Null Object!");
                }
                var newReport = new Reports(report.SenderId, report.ReportName, report.ReportedType, report.ReportedEntityId, report.TargetType, report.Message ?? "");
                newReport.CreateBy = report.CreateBy;
                newReport.UpdateBy = report.CreateBy;
                _reportsService.Add(newReport);
                return StatusCode(201, newReport.ReportsId);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        [ViLAuthorize(Role = "Admin")]
        public IActionResult Update(Reports report)
        {
            try
            {
                var query = _reportsService.GetById(report.ReportsId);
                if (query == null )
                {
                    return StatusCode(404, $"Not found report with id {report.ReportsId}.");
                }
                _reportsService.Update(report);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        [ViLAuthorize(Role = "Admin")]
        public IActionResult Delete(string id)
        {
            try
            {
                var query = _reportsService.GetById(id);
                if (query == null )
                {
                    return StatusCode(404, $"Not found report with id {id}.");
                }
                _reportsService.Delete(query);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

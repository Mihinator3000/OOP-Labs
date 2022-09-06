using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reports.Common.DataTransferObjects;
using Reports.Common.Tools;
using Reports.DataAccessLayer.Services.Interfaces;

namespace Reports.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService ?? throw new NullReferenceException(nameof(reportService));
        }

        [HttpGet]
        [Route("get/all")]
        public async Task<ActionResult<List<ReportDto>>> GetAll()
        {
            return Ok(await _reportService.GetAll());
        }

        [HttpGet]
        [Route("get/user/{userId}")]
        public async Task<ActionResult<List<ReportDto>>> GetByUserId(int userId)
        {
            try
            {
                return Ok(await _reportService.GetByUserId(userId));
            }
            catch (ReportsDbException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult<ReportDto>> GetById(int id)
        {
            try
            {
                return Ok(await _reportService.GetById(id));
            }
            catch (ReportsDbException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("create/user/{userId}")]
        public async Task<ActionResult> Create([FromBody] ReportDto report, int userId)
        {
            try
            {
                await _reportService.Create(report, userId);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("addTask/{id}/{taskId}")]
        public async Task<ActionResult> AddTask(int id, int taskId)
        {
            try
            {
                await _reportService.AddTask(id, taskId);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update(ReportDto report)
        {
            try
            {
                await _reportService.Update(report);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("close/{id}")]
        public async Task<ActionResult> CloseReport(int id)
        {
            try
            {
                await _reportService.Close(id);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("get/weektasks")]
        public async Task<ActionResult<List<TaskDto>>> GetTasksForTheWeek()
        {
            return Ok(await _reportService.GetTasksForTheWeek());
        }

        [HttpGet]
        [Route("subordinates/{userId}")]
        public async Task<ActionResult<List<ReportDto>>> SubordinatesReports(int userId)
        {
            try
            {
                await _reportService.SubordinatesReports(userId);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("subordinates/without/{userId}")]
        public async Task<ActionResult<List<UserDto>>> SubordinatesWithoutReport(int userId)
        {
            try
            {
                await _reportService.SubordinatesWithoutReport(userId);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return NotFound();
            }
        }
    }
}

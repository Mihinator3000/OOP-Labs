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
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService ?? throw new NullReferenceException(nameof(taskService));
        }

        [HttpGet]
        [Route("get/all")]
        public async Task<ActionResult<List<TaskDto>>> GetAll()
        {
            return Ok(await _taskService.GetAll());
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult<TaskDto>> GetById(int id)
        {
            try
            {
                return Ok(await _taskService.GetById(id));
            }
            catch (ReportsDbException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("get/creation")]
        public async Task<ActionResult<List<TaskDto>>> GetForCreation([FromBody] DateTimeDto time)
        {
            return Ok(await _taskService.GetForCreation(time));
        }

        [HttpGet]
        [Route("get/lastChange")]
        public async Task<ActionResult<List<TaskDto>>> GetForLastChange([FromBody] DateTimeDto time)
        {
            return Ok(await _taskService.GetForLastChange(time));
        }

        [HttpGet]
        [Route("get/user/{userId}")]
        public async Task<ActionResult<List<TaskDto>>> GetForUser(int userId)
        {
            return Ok(await _taskService.GetForUser(userId));
        }

        [HttpGet]
        [Route("get/userChanges/{userId}")]
        public async Task<ActionResult<List<TaskDto>>> GetForUserChanges(int userId)
        {
            return Ok(await _taskService.GetForUserChanges(userId));
        }

        [HttpGet]
        [Route("get/userSubordinates/{userId}")]
        public async Task<ActionResult<List<TaskDto>>> GetForSubordinates(int userId)
        {
            try
            {
                return Ok(await _taskService.GetForSubordinates(userId));
            }
            catch (ReportsDbException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create([FromBody] TaskDto task)
        {
            try
            {
                await _taskService.Create(task);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("update/user/{userId}")]
        public async Task<ActionResult> Update([FromBody] TaskDto task, int userId)
        {
            try
            {
                await _taskService.Update(task, userId);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _taskService.Delete(id);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("comment/{id}/user/{userId}")]
        public async Task<ActionResult> AddComment([FromBody] CommentDto comment, int id, int userId)
        {
            try
            {
                await _taskService.AddComment(id, userId, comment);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("assign/{id}/user/{userId}")]
        public async Task<ActionResult> ChangeAssignedUser(int id, int userId)
        {
            try
            {
                await _taskService.ChangeAssignedUser(id, userId);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return BadRequest();
            }
        }
    }
}
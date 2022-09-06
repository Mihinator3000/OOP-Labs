using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Common.DataTransferObjects;
using Reports.Common.Tools;
using Reports.DataAccessLayer.Services.Interfaces;

namespace Reports.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new NullReferenceException(nameof(userService));
        }

        [HttpGet]
        [Route("get/all")]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            return Ok(await _userService.GetAll());
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult<UserInfoDto>> GetById(int id)
        {
            try
            {
                return Ok(await _userService.GetById(id));
            }
            catch (ReportsDbException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create([FromBody]UserDto user)
        {
            try
            {
                await _userService.Create(user);
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
                await _userService.Delete(id);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody]UserDto user)
        {
            try
            {
                await _userService.Update(user);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return NotFound();
            }
        }
    }
}

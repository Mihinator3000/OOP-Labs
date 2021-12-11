using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Reports.Common.DataTransferObjects;
using Reports.Common.Tools;
using Reports.DataAccessLayer.Services.Interfaces;

namespace Reports.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new NullReferenceException(nameof(authService));
        }

        [HttpGet]
        [Route("getId")]
        public async Task<ActionResult<int>> GetId([FromBody] LoginDto info)
        {
            try
            {
                return Ok(await _authService.GetId(info));
            }
            catch (ReportsDbException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateAccount([FromBody] LoginDto info)
        {
            try
            {
                await _authService.CreateAccount(info);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("delete/{userId}")]
        public async Task<ActionResult> DeleteAccount(int userId)
        {
            try
            {
                await _authService.DeleteAccount(userId);
                return Ok();
            }
            catch (ReportsDbException)
            {
                return NotFound();
            }
        }
    }
}

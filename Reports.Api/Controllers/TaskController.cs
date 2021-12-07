using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reports.DataAccessLayer;
using Reports.DataAccessLayer.Entities;

namespace Reports.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ReportsContext _context;

        public TaskController(ReportsContext context)
        {
            _context = context ?? throw new NullReferenceException(nameof(context));
            /*if (!_context.Tasks.Any())
            {
                _context.Tasks.Add(new DbTask { State = 10 });
                _context.Tasks.Add(new DbTask { State = 20 });
                _context.SaveChanges();
            }*/
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DbTask>>> Get()
        {
            return await _context.Tasks.ToListAsync();
        }

        // GET api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DbTask>> Get(int id)
        {
            DbTask user = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return NotFound();
            return new ObjectResult(user);
        }

        // POST api/Tasks
        [HttpPost]
        public async Task<ActionResult<DbTask>> Post(DbTask user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            _context.Tasks.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // PUT api/Tasks/
        [HttpPut]
        public async Task<ActionResult<DbTask>> Put(DbTask user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!_context.Tasks.Any(x => x.Id == user.Id))
            {
                return NotFound();
            }

            _context.Update(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // DELETE api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DbTask>> Delete(int id)
        {
            DbTask user = _context.Tasks.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Tasks.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
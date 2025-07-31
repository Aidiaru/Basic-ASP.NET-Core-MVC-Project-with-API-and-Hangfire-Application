using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LogsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LogsApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Log>>> Get()
        {
            var logs = await _context.Logs.ToListAsync();
            return Ok(logs);
        }

        [HttpGet]
        public async Task<ActionResult<PagedLogResult>> GetPaged(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? search = null)
        {
            var query = _context.Logs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(log =>
                    log.Message.Contains(search) ||
                    log.Level.Contains(search) ||
                    log.Logger.Contains(search));
            }

            var totalCount = await query.CountAsync();

            var logs = await query
                .OrderByDescending(l => l.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PagedLogResult
            {
                Logs = logs,
                TotalCount = totalCount
            };

            return Ok(result);
        }




        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Log log)
        {

            if (log.Date == default)
                log.Date = DateTime.Now;

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
            return Ok(log);
        }
    }
}
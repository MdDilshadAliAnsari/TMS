using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.Logger.Model;

namespace TMS.Logger.Controllers
{
    [ApiController]
    [Route("api/v{1:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class LogsController : ControllerBase
    {
        private readonly TMSDbContext _context;

        public LogsController(TMSDbContext context)
        {
            _context = context;
        }
        [HttpPost("Loggers")] 
        public IActionResult LogEntry([FromBody] LOGGERS log)
        {
            if (log is null)
            {
                return BadRequest("Invalid user request!!!");
            }
            log.LoggerId = null;
            _context.LOGGER.Add(log);
            _context.SaveChanges();

            return Ok(new { message = "Successfully Save Logs!!!" });
        }
    }
}

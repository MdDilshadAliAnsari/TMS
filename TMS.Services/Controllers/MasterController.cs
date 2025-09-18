using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.Services.Model;

namespace TMS.Services.Controllers
{
    [ApiController]
    [Route("api/v{1:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MasterController : ControllerBase
    {
        #region  Variable Constructor and Dependency injection in Constructor level
        private readonly TMSDbContext _context;
        public MasterController(TMSDbContext context)
        {
            _context = context;
        }
        #endregion
        #region Populate DropDown 
        [HttpGet("TASKSPRIORITY")]
        public IActionResult TSPRIORITY()
        {
            var result = _context.TASKSPRIORITIES.Select(e => new { e.NAME, e.TASKSPRIORITYID }).ToListAsync();
            if (result == null)
            {        // Handle case when the Data is not found
                return Ok(new { message = "Record not found!!!" });
            }
            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("TASKSTATUS")]
        public IActionResult TSTATUS()
        {

            var result = _context.STATUS.Select(e => new { e.NAME, e.STATUSID }).ToListAsync();
            if (result == null)
            {
                // Handle case when the Data is not found
                return Ok(new { message = "Record not found!!!" });
            }
            var response = new { result };
            return Ok(new { response });


        }
        [HttpGet("TASKCATEGORY")]
        public IActionResult TCATEGORY()
        {

            var result = _context.TASKCATEGORIES.Select(e => new { e.NAME, e.TASKCATEGORYID }).ToListAsync();
            if (result == null)
            {
                // Handle case when the Data is not found
                return Ok(new { message = "Record not found!!!" });
            }
            var response = new { result };
            return Ok(new { response });


        }

        [HttpGet("PROJECTLIST")]
        public IActionResult PROJECTLIST()
        {
            var result = _context.Projects.Select(e => new { e.PROJECTNAME, e.PROJECTID }).ToListAsync();
            if (result == null)
            {
                // Handle case when the Data is not found
                return Ok(new { message = "Record not found!!!" });
            }
            var response = new { result };
            return Ok(new { response });


        }

        #endregion
    }
}

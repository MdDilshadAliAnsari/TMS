using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TMS.Services.Model;

namespace TMS.Services.Controllers
{
    [ApiController]
    [Route("api/v{1:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class StatusController : ControllerBase
    {
        #region  Variable Constructor and Dependency injection in Constructor level
        private readonly TMSDbContext _context;
        public StatusController(TMSDbContext context)
        {
            _context = context;
        }
        #endregion
        #region Operation on Tasks
        [HttpGet("GetAllStatus")]
        public IActionResult GetProjCategory()
        {

            var result = _context.STATUS.Where(p => p.ISDELETED == 0).ToList();
            var response = new { result };
            return Ok(new { response });
        }

        [HttpGet("GetStatus")]
        public IActionResult GetProjCategory(int statusId)
        {
            if (statusId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var result = _context.STATUS.Where(m => m.STATUSID == statusId).ToList(); 
            var response = new { result };
            return Ok(new { response });
        }

        [HttpPost("Newstatus")]
        public IActionResult Newstatus([FromBody] STATUS sts)
        {
            if (sts is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            sts.STATUSID = null;
            sts.ISDELETED = 0;
            _context.STATUS.Add(sts);
            _context.SaveChanges();
            return Ok(new { message = "TASK Status Successfully Added!!! " });
        }

        [HttpPatch("Updstatus")]
        public IActionResult Updstatus([FromBody] STATUS sts)
        {
            if (sts is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Task = _context.STATUS.Find(sts.STATUSID);

            if (_Task != null)
            {
                _context.Entry(_Task).State = EntityState.Detached;
                // Step 2: Modify the Task property
                _Task.NAME        = sts.NAME;
                _Task.DESCRIPTION = sts.DESCRIPTION;
                _Task.ISDELETED = 0;
                _Task.CREATEDBY = sts.CREATEDBY;
                _Task.CREATEDON = sts.CREATEDON;
                _Task.UPDATEDBY = sts.UPDATEDBY;
                _Task.UPDATEDON = sts.UPDATEDON;

                // Step 3: Save the changes to the database 
                _context.STATUS.Update(sts);
                _context.SaveChangesAsync();

                return Ok(new { message = "TASK Status Successfully Modified!!!" + sts.STATUSID });
            }
            else
            {
                // Handle case when the Project is not found
                return Ok(new { message = "TASK Status not found!!!" });

            }
        }

        [HttpPatch("Delstatus")]
        public IActionResult DelTASKCATEGORY([FromBody] STATUS tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Task = _context.STATUS.Find(tsk.STATUSID);

            if (_Task != null)
            {
                _context.Entry(_Task).State = EntityState.Detached;
                // Step 2: Modify the Project property
                _Task.ISDELETED = 1;
                _Task.CREATEDBY = tsk.CREATEDBY;
                _Task.CREATEDON = tsk.CREATEDON;
                _Task.UPDATEDBY = tsk.UPDATEDBY;
                _Task.UPDATEDON = tsk.UPDATEDON;

                // Step 3: Save the changes to the database
                _context.STATUS.Update(tsk);
                _context.SaveChangesAsync();

                return Ok(new { message = "TASK Status  Successfully Deleted!!! " + tsk.STATUSID });
            }
            else
            {
                // Handle case when the Project is not found
                return Ok(new { message = "TASK Status not found!!!" });
            }
        }
        #endregion
    }
}

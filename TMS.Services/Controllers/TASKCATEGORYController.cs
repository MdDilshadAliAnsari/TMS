using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.Services.Model;

namespace TMS.Services.Controllers
{
    [ApiController]
    [Route("api/v{1:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TASKCATEGORYController : ControllerBase
    {
       
        #region  Variable Constructor and Dependency injection in Constructor level
        private readonly TMSDbContext _context;
        public TASKCATEGORYController(TMSDbContext context)
        {
            _context = context;
        }
        #endregion
        #region Operation on Tasks
        [HttpGet("GetAllTASKCATEGORY")]
        public IActionResult GetProjCategory()
        {

            var result = _context.TASKCATEGORIES.Where(p => p.ISDELETED == 0).ToList();
            var response = new { result };
            return Ok(new { response });
        }

        [HttpGet("GetTASKCATEGORY")]
        public IActionResult GetProjCategory(int TASKCATEGORYId)
        {
            if (TASKCATEGORYId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var result = _context.TASKCATEGORIES.Where(m => m.TASKCATEGORYID == TASKCATEGORYId).ToList();
            var response = new { result };
            return Ok(new { response });
        }

        [HttpPost("NewTASKCATEGORY")]
        public IActionResult NewTASKCATEGORY([FromBody] TASKCATEGORY tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            tsk.TASKCATEGORYID = null;
            tsk.ISDELETED = 0;
            _context.TASKCATEGORIES.Add(tsk);
            _context.SaveChanges();
            return Ok(new { message = "TASK CATEGORY Successfully Added!!! " });
        }

        [HttpPatch("UpdTASKCATEGORY")]
        public IActionResult UpdTASKCATEGORY([FromBody] TASKCATEGORY tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Task = _context.TASKCATEGORIES.Find(tsk.TASKCATEGORYID);

            if (_Task != null)
            {
                _context.Entry(_Task).State = EntityState.Detached;
                // Step 2: Modify the Task property
                _Task.NAME          = tsk.NAME;
                _Task.DESCRIPTION   = tsk.DESCRIPTION;
                _Task.ISDELETED     = 0;
                _Task.CREATEDBY     = tsk.CREATEDBY;
                _Task.CREATEDON     = tsk.CREATEDON;
                _Task.UPDATEDBY     = tsk.UPDATEDBY;
                _Task.UPDATEDON     = tsk.UPDATEDON;

                // Step 3: Save the changes to the database
                _context.TASKCATEGORIES.Update(_Task);
                _context.SaveChangesAsync();

                return Ok(new { message = "TASK CATEGORIES Successfully Modified!!!" + tsk.TASKCATEGORYID });
            }
            else
            {
                // Handle case when the Project is not found

                return Ok(new { message = "TASK CATEGORIES not found!!!" });
            }
        }

        [HttpPatch("DelTASKCATEGORY")]
        public IActionResult DelTASKCATEGORY([FromBody] Tassk tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Task = _context.TASKCATEGORIES.Find(tsk.TASKCATEGORYID);

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
                _context.TASKCATEGORIES.Update(_Task);
                _context.SaveChangesAsync();

                return Ok(new { message = "TASK CATEGORY  Successfully Deleted!!! " + tsk.TASKCATEGORYID });
            }
            else
            {
                // Handle case when the Project is not found

                return Ok(new { message = "TASK CATEGORY not found!!!" });
            }
        }
        #endregion
    }}

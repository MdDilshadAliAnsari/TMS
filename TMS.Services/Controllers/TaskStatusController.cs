using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TMS.Services.Model;

namespace TMS.Services.Controllers
{
    [ApiController]
    [Route("api/v{1:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TaskStatusController : ControllerBase
    {

        #region  Variable Constructor and Dependency injection in Constructor level
        private readonly TMSDbContext _context;
        public TaskStatusController(TMSDbContext context)
        {
            _context = context;
        }
        #endregion
        #region Operation on Tasks
        [HttpGet("GetAllTaskStatus")]
        public IActionResult GetTask()
        { 
            var result = _context.TaskStatuses.Where(p => p.ISDELETED == 0).ToList();
            var response = new { result };
            return Ok(new { response });
        }

        [HttpGet("GetTaskStatus")]
        public IActionResult GetTask(int TaskstatusId)
        {
            if (TaskstatusId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var result = _context.TaskStatuses.Where(m => m.TASKSSTATUSID == TaskstatusId).ToList();
            var response = new { result };
            return Ok(new { response });
        }

        [HttpPost("NewTskStatus")]
        public IActionResult NewTask([FromBody] TasskStatus tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            tsk.TASKSSTATUSID = null;
            tsk.ISDELETED = 0;
            _context.TaskStatuses.Add(tsk);
            _context.SaveChanges();
            return Ok(new { message = "Task status Successfully Added!!! "});
        }

        [HttpPatch("UpdTskStatus")]
        public IActionResult UpdTask([FromBody] TasskStatus tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Task = _context.TaskStatuses.Find(tsk.TASKSSTATUSID);

            if (_Task != null)
            {   _context.Entry(_Task).State = EntityState.Detached;
                // Step 2: Modify the Task property 
                _Task.STATUSID          = tsk.STATUSID;
                _Task.DESCRIPTION       = tsk.DESCRIPTION; 
                _Task.TASKCATEGORYID    = tsk.TASKCATEGORYID;
                _Task.TASKPRIORITYID    = tsk.TASKPRIORITYID;
                _Task.ISDELETED         = 0 ; 
                _Task.CREATEDBY         = tsk.CREATEDBY;
                _Task.CREATEDON         = tsk.CREATEDON;
                _Task.UPDATEDBY         = tsk.UPDATEDBY;
                _Task.UPDATEDON         = tsk.UPDATEDON;

                // Step 3: Save the changes to the database
                _context.TaskStatuses.Update(_Task); 
                _context.SaveChangesAsync();

                return Ok(new { message = "Task status Successfully Modified!!! " + tsk.TASKSSTATUSID });
            }
            else
            {
                // Handle case when the Project is not found
                return Ok(new { message = "Task status  not found!!!" });
            }
        }

        [HttpPatch("DelTskStatus")]
        public IActionResult DelTask([FromBody] TasskStatus tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Task = _context.TaskStatuses.Find(tsk.TASKSSTATUSID);

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
                _context.TaskStatuses.Update(_Task);
                _context.SaveChangesAsync();

                return Ok(new { message = "Task status Successfully Deleted !!!" + tsk.TASKSSTATUSID });
            }
            else
            {
                // Handle case when the Project is not found
                return Ok(new { message = "Task status  not found!!!" });
            }
        }
        #endregion
    } 
}

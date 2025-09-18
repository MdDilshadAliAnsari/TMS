using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.Services.Model;

namespace TMS.Services.Controllers
{
    [ApiController]
    [Route("api/v{1:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CommentController : ControllerBase
    {
        #region  Variable Constructor and Dependency injection in Constructor level
        private readonly TMSDbContext _context;
        public CommentController(TMSDbContext context)
        {
            _context = context;
        }
        #endregion
        #region Operation on Tasks
        [HttpGet("GetAllComment")]
        public IActionResult GetCommen(int TaskId)
        {
            if (TaskId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var result = _context.Comments.Where(m => m.TASKID == TaskId && m.ISDELETED == 0).ToList();
            if (result == null)
            {
                // Handle case when the Document is not found
                return Ok(new { message = "Comment not found!!!" });
            }
            var response = new { result };
            return Ok(new { response });
        }

        [HttpGet("GetComment")]
        public IActionResult GetCommen(int TaskId,int CommentId)
        {
            if (TaskId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var result = _context.Comments.Where(m => m.TASKID == CommentId && m.COMMENTID == CommentId && m.ISDELETED ==0).ToList(); 
            if (result == null)
            {
                // Handle case when the Document is not found
                return Ok(new { message = "Comment not found!!!" });
            }
            var response = new { result };
            return Ok(new { response });
        }

        [HttpPost("NewComment")]
        public IActionResult NewComment([FromBody] Comment tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            tsk.COMMENTID = null;
            tsk.ISDELETED = 0;
            _context.Comments.Add(tsk);
            _context.SaveChanges();
            return Ok(new { message = "Task Comments Successfully Added!!! " });
        }

        [HttpPatch("UpdComment")]
        public IActionResult UpdComment([FromBody] Comment tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Task = _context.Comments.Find(tsk.COMMENTID);

            if (_Task != null)
            {
                _context.Entry(_Task).State = EntityState.Detached;
                // Step 2: Modify the Task property
                _Task.TASKID        = tsk.TASKID;
                _Task.USERID        = tsk.USERID;
                _Task.COMMENTTEXT   = tsk.COMMENTTEXT; 
                _Task.ISDELETED = 0;
                _Task.CREATEDBY = tsk.CREATEDBY;
                _Task.CREATEDON = tsk.CREATEDON;
                _Task.UPDATEDBY = tsk.UPDATEDBY;
                _Task.UPDATEDON = tsk.UPDATEDON;

                // Step 3: Save the changes to the database
                _context.Comments.Update(_Task);
                _context.SaveChangesAsync();

                return Ok(new { message = "Task Comments Successfully Modified!!!" + tsk.COMMENTID });
            }
            else
            {
                // Handle case when the Project is not found
                return Ok(new { message = "Task comment not found!!!" });
            }
        }

        [HttpPatch("DelComments")]
        public IActionResult DelComments([FromBody] Comment tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Task = _context.Comments.Find(tsk.COMMENTID);

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
                _context.Comments.Update(_Task);
                _context.SaveChangesAsync();

                return Ok(new { message = "Task Comments Successfully Deleted!!! " + tsk.TASKID });
            }
            else
            {
                // Handle case when the Project is not found
                return Ok(new { message = "Task comment not found!!!" });
            }
        }
        #endregion
    }
}

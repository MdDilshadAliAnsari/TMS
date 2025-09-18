using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TMS.Services.Model;

namespace TMS.Services.Controllers
{
    [ApiController]
    [Route("api/v{1:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProjectController : ControllerBase
    {
        #region  Variable Constructor and Dependency injection in Constructor level
        private readonly TMSDbContext _context;
        public ProjectController(TMSDbContext context)
        {
            _context = context;
        }
        #endregion
        #region Operation on Project
        [HttpGet("GetAllProject")]
        public IActionResult GetProj()
        {
           
            var result = _context.Projects.Where(p => p.ISDELETED == 0); 
            var response = new { result };
            return Ok(new { response });
        }

        [HttpGet("GetProject")]
        public IActionResult GetProj(int ProjId)
        {
            if (ProjId ==0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var result = _context.Projects.Where(m => m.PROJECTID == ProjId).ToList();
            var response = new { result };
            return Ok(new { response });
        }

        [HttpPost("NewProject")]
        public IActionResult NewProj([FromBody] Project Proj)
        {
            if (Proj is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            Proj.PROJECTID = null;
            Proj.ISDELETED = 0;
            _context.Projects.Add(Proj);
            _context.SaveChanges();
            return Ok(new { message = "Successfully Save Project!!!" });
        }

        [HttpPatch("UpdProject")]
        public IActionResult UpdProj([FromBody] Project Proj)
        {
            if (Proj is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Proj = _context.Projects.Find(Proj.PROJECTID);
            //_context.Projects.FindAsync(Proj.PROJECTID);

            if (_Proj != null)
            {
                _context.Entry(_Proj).State = EntityState.Detached;
                // Step 2: Modify the Project property
                _Proj.PROJECTNAME = Proj.PROJECTNAME;
                _Proj.DESCRIPTION = Proj.DESCRIPTION;
                _Proj.STARTDATE = Proj.STARTDATE;
                _Proj.ISDELETED = 0;
                _Proj.ENDDATE = Proj.ENDDATE;
                _Proj.CREATEDBY = Proj.CREATEDBY;
                _Proj.CREATEDON = Proj.CREATEDON;
                _Proj.UPDATEDBY = Proj.UPDATEDBY;
                _Proj.UPDATEDON = Proj.UPDATEDON;

                // Step 3: Save the changes to the database
                _context.Projects.Update(_Proj); 
                _context.SaveChangesAsync();

                return Ok(new { message = "Project Successfully Modified!!! " + Proj.PROJECTID} );
            }
            else
            {
                // Handle case when the Project is not found                
                return Ok(new { message = "Project not found!!!" });
            } 
        }

        [HttpPatch("DelProject")]
        public IActionResult DelProj([FromBody] Project Proj)
        {
            if (Proj is null)
            { 
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            if (Proj.PROJECTID==0)
            { 
                return BadRequest(new { message = "Invalid Project!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Proj = _context.Projects.Find(Proj.PROJECTID);

            if (_Proj != null)
            {
                // Step 2: Modify the Project property
                _context.Entry(_Proj).State = EntityState.Detached;
                _Proj.ISDELETED = 1;
                _Proj.CREATEDBY = Proj.CREATEDBY;
                _Proj.CREATEDON = Proj.CREATEDON;
                _Proj.UPDATEDBY = Proj.UPDATEDBY;
                _Proj.UPDATEDON = Proj.UPDATEDON;

                // Step 3: Save the changes to the database
                _context.Projects.Update(_Proj);
                _context.SaveChangesAsync();

                return Ok(new { message = "Project Successfully Deleted!!! " + Proj.PROJECTID });
            }
            else
            {
                // Handle case when the Project is not found
                return Ok(new { message = "Project not found!!!" });
            }
        }
        #endregion
    }
}

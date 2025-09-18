using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TMS.Services.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Math;
using Comment = TMS.Services.Model.Comment;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Http.HttpResults;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;

namespace TMS.Services.Controllers
{
    [ApiController]
    [Route("api/v{1:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TaskController : ControllerBase
    {
        #region  Variable Constructor and Dependency injection in Constructor level
        private readonly TMSDbContext _context;
        private readonly string _mailAttachmentPath;
        private readonly string _TaskTemplatePath;
        public TaskController(TMSDbContext context, IConfiguration configuration)
        {
            _context = context;
            _mailAttachmentPath = configuration["DocumentUrl:MailAttachmentPath"];
            _TaskTemplatePath = configuration["DocumentUrl:TaskTemplatePath"];
        }
        #endregion
        #region Operation on Tasks
        [HttpGet("GetAllTask")]
        public IActionResult GetTask(int UserId)
        {
            #region commented part
            //var result = from t in _context.Tasks
            //             join p in _context.Projects on t.PROJECTID equals p.PROJECTID
            //             join pri in _context.TASKSPRIORITIES on t.TASKSPRIORITYID equals pri.TASKSPRIORITYID
            //             join cat in _context.TASKCATEGORIES on t.TASKCATEGORYID equals cat.TASKCATEGORYID
            //             select new
            //             {
            //                 MainId = t.TASKID,

            //                 SenderEmail = _context.TaskEmailS
            //                     .Where(e => e.TASKID == t.TASKID && e.TASKSERIALNO == t.TASKSERIALNO)
            //                     .OrderBy(e => e.CREATEDON)
            //                     .Select(e => e.SENDEREMAILADDRESS)
            //                     .FirstOrDefault(),

            //                 StatusId = _context.TaskStatuses
            //                     .Where(e => e.TASKID == t.TASKID && e.TASKSERIALNO == t.TASKSERIALNO && e.ISDELETED == 0)
            //                     .OrderByDescending(e => e.CREATEDON)
            //                     .Select(e => e.STATUSID)
            //                     .FirstOrDefault(),

            //                 Status = _context.STATUS
            //                     .Where(s => s.STATUSID ==
            //                         _context.TaskStatuses
            //                             .Where(e => e.TASKID == t.TASKID && e.TASKSERIALNO == t.TASKSERIALNO && e.ISDELETED == 0)
            //                             .OrderByDescending(e => e.CREATEDON)
            //                             .Select(e => e.STATUSID)
            //                             .FirstOrDefault()
            //                     )
            //                     .Select(s => s.NAME)
            //                     .FirstOrDefault(),

            //                 CategoryName = cat.NAME,
            //                 category = cat.DESCRIPTION,

            //                 ProjectName = p.PROJECTNAME,
            //                 priority = pri.DESCRIPTION,
            //                 PriorityDescription = pri.DESCRIPTION,

            //                 assign = _context.Set<UserNameResultDTO>()
            //                     .FromSqlRaw(
            //                         "EXEC [TMS].[USP_GetUserName] @UserId = {0}",
            //                         _context.TaskAssigned
            //                             .Where(e => e.TASKID == t.TASKID && e.TASKSERIALNO == t.TASKSERIALNO && e.ISDELETED == 0)
            //                             .OrderByDescending(e => e.CREATEDON)
            //                             .Select(e => e.TASKASSIGNID)
            //                             .FirstOrDefault()
            //                     )
            //                     .AsEnumerable()
            //                     .FirstOrDefault()?.UserName ?? "N/A",

            //                 Task = t
            //             };

            #endregion
           
            var projectList = _context.ProjectAssign.Where(e => e.USERID == UserId && e.ISDELETED == 0).ToList();
            // Get all project IDs assigned to this user
            var projectIds  = projectList.Select(p => p.PROJECTID).ToList();

            var taskList = (from t      in _context.Tasks
                            join p      in _context.Projects        on t.PROJECTID          equals p.PROJECTID
                            join pri    in _context.TASKSPRIORITIES on t.TASKSPRIORITYID    equals pri.TASKSPRIORITYID
                            join cat    in _context.TASKCATEGORIES  on t.TASKCATEGORYID     equals cat.TASKCATEGORYID
                            where projectIds.Contains(t.PROJECTID)
                            select new
                            {
                                Task = t,
                                ProjectName             = p.PROJECTNAME,
                                PriorityDescription     = pri.DESCRIPTION,
                                CategoryName            = cat.NAME,
                                CategoryDesc            = cat.DESCRIPTION
                            }).OrderByDescending(x => x.Task.TASKID).ToList(); // ⬅ break the expression tree here



            var result = taskList.Select(t => new
            {
                MainId = t.Task.TASKID,

                SenderEmail = _context.TaskEmailS
                                                        .Where(e => e.TASKID == t.Task.TASKID && e.TASKSERIALNO == t.Task.TASKSERIALNO)
                                                        .OrderBy(e => e.CREATEDON)
                                                        .Select(e => e.SENDEREMAILADDRESS)
                                                        .FirstOrDefault(),

          

                Status = _context.STATUS
                                                        .Where(s => s.STATUSID ==
                                                            _context.TaskStatuses
                                                                .Where(e => e.TASKID == t.Task.TASKID && e.TASKSERIALNO == t.Task.TASKSERIALNO && e.ISDELETED == 0)
                                                                .OrderByDescending(e => e.CREATEDON)
                                                                .Select(e => e.STATUSID)
                                                                .FirstOrDefault()
                                                        )
                                                        .Select(s => s.NAME)
                                                        .FirstOrDefault(),
                StatusId = _context.TaskStatuses
                                                        .Where(e => e.TASKID == t.Task.TASKID && e.TASKSERIALNO == t.Task.TASKSERIALNO && e.ISDELETED == 0)
                                                        .OrderByDescending(e => e.TASKID)
                                                        .Select(e => e.STATUSID)
                                                        .FirstOrDefault(),
                CategoryName = t.CategoryName,
                ProjectName = t.ProjectName,
                PriorityDescription = t.PriorityDescription,
                category = t.CategoryDesc,
                priority = t.PriorityDescription,
                assign = _context.Set<UserNameResultDTO>()
                                                        .FromSqlRaw(
                                                            "EXEC [TMS].[USP_GetUserName] @UserId = {0}",
                                                            _context.TaskAssigned
                                                                .Where(e => e.TASKID == t.Task.TASKID && e.TASKSERIALNO == t.Task.TASKSERIALNO && e.ISDELETED == 0)
                                                                .OrderByDescending(e => e.TASKID)
                                                                .Select(e => e.WHICHDEVELOPERASSIGNED)
                                                                .FirstOrDefault()
                                                        )
                                                        .AsEnumerable()
                                                        .FirstOrDefault()?.UserName ?? "N/A",  // now safe outside EF expression
                task                                = t.Task
            });  


            var response = new { result };
            return Ok(new { response });
        }

        [HttpGet("GetTask")]
        public IActionResult GetTask(int TaskId, int UserId)
        { 
            if (TaskId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            var projectList = _context.ProjectAssign.Where(e => e.USERID == UserId && e.ISDELETED == 0).ToList();
            // Get all project IDs assigned to this user
            var projectIds = projectList.Select(p => p.PROJECTID).ToList();
            //var result = _context.Tasks.Where(m => m.TASKID == TaskId).ToList();
            //var response = new { result };

            var assignedDev = _context.TaskAssigned
                                .Where(e => e.TASKID == TaskId)
                                .OrderByDescending(e => e.CREATEDON)
                                .Select(e => e.WHICHDEVELOPERASSIGNED)
                                .FirstOrDefault();

            var result = from t in _context.Tasks
                         join p in _context.Projects on t.PROJECTID equals p.PROJECTID
                         join pri in _context.TASKSPRIORITIES on t.TASKSPRIORITYID equals pri.TASKSPRIORITYID
                         join cat in _context.TASKCATEGORIES on t.TASKCATEGORYID equals cat.TASKCATEGORYID
                         join tst in _context.TaskStatuses on t.TASKID equals tst.TASKID
                         join st in _context.STATUS on tst.STATUSID equals st.STATUSID
                         where t.TASKID == TaskId  
                         orderby tst.TASKSSTATUSID descending
                         select new
                         {
                             MainId = t.TASKID,
                             SenderEmail = _context.TaskEmailS
                                .Where(e => e.TASKID == t.TASKID && e.TASKSERIALNO == t.TASKSERIALNO)
                                .OrderByDescending(e => e.CREATEDON)
                                .Select(e => e.SENDEREMAILADDRESS)
                                .FirstOrDefault(),

                             Assign     = assignedDev != null ? "Assign" : "unassign",

                             Category   = _context.TASKCATEGORIES
                                            .Where(e => e.TASKCATEGORYID == t.TASKCATEGORYID)
                                            .Select(e => e.NAME)
                                            .FirstOrDefault(),
                             Priority = _context.TASKSPRIORITIES.Where(e => e.TASKSPRIORITYID == t.TASKSPRIORITYID)
                                        .Select(e => e.NAME)
                                     .FirstOrDefault(),
                             Status                 = st.NAME,
                             StatusId               = st.STATUSID,
                             CategoryName           = cat.NAME,
                             ProjectName            = p.PROJECTNAME,
                             PriorityDescription    = pri.DESCRIPTION,

                             Task = t

                         };
            var response = new { result };
            return Ok(new { response });
        }






        [HttpGet("GetAllAssignTask")]
        public IActionResult GetAllAssignTask(int UserId)
        {

            var projectList = _context.ProjectAssign.Where(e => e.USERID == UserId && e.ISDELETED == 0).ToList();
            // Get all project IDs assigned to this user
            var projectIds = projectList.Select(p => p.PROJECTID).ToList();


            // Get related tasks (ISDELETED == 0 only)
            var taskIdsWithLatestAssignment = _context.TaskAssigned.Where(t => t.ISDELETED == 0).Select(x => x.TASKID).ToList();

            var taskList = (from t in _context.Tasks
                            where taskIdsWithLatestAssignment.Contains(t.TASKID)
                            join p in _context.Projects on t.PROJECTID equals p.PROJECTID
                            join pri in _context.TASKSPRIORITIES on t.TASKSPRIORITYID equals pri.TASKSPRIORITYID
                            join cat in _context.TASKCATEGORIES on t.TASKCATEGORYID equals cat.TASKCATEGORYID
                            where projectIds.Contains(t.PROJECTID)
                            select new
                            {
                                Task = t,
                                ProjectName = p.PROJECTNAME,
                                PriorityDescription = pri.DESCRIPTION,
                                CategoryName = cat.NAME,
                                CategoryDesc = cat.DESCRIPTION
                            }).OrderByDescending(x => x.Task.TASKID).ToList(); // ⬅ break the expression tree here


         
           

            var result = taskList.Select(t => new
            {
                MainId = t.Task.TASKID,

                SenderEmail = _context.TaskEmailS
                                                        .Where(e => e.TASKID == t.Task.TASKID && e.TASKSERIALNO == t.Task.TASKSERIALNO)
                                                        .OrderBy(e => e.TASKID)
                                                        .Select(e => e.SENDEREMAILADDRESS)
                                                        .FirstOrDefault(),



                Status = _context.STATUS
                                                        .Where(s => s.STATUSID ==
                                                            _context.TaskStatuses
                                                                .Where(e => e.TASKID == t.Task.TASKID && e.TASKSERIALNO == t.Task.TASKSERIALNO && e.ISDELETED == 0)
                                                                .OrderByDescending(e => e.TASKID)
                                                                .Select(e => e.STATUSID)
                                                                .FirstOrDefault()
                                                        )
                                                        .Select(s => s.NAME)
                                                        .FirstOrDefault(),
                StatusId = _context.TaskStatuses
                                                        .Where(e => e.TASKID == t.Task.TASKID && e.TASKSERIALNO == t.Task.TASKSERIALNO && e.ISDELETED == 0)
                                                        .OrderByDescending(e => e.TASKID)
                                                        .Select(e => e.STATUSID)
                                                        .FirstOrDefault(),
                CategoryName = t.CategoryName,
                ProjectName = t.ProjectName,
                PriorityDescription = t.PriorityDescription,
                category = t.CategoryDesc,
                priority = t.PriorityDescription,
                assign = _context.Set<UserNameResultDTO>()
                                                        .FromSqlRaw(
                                                            "EXEC [TMS].[USP_GetUserName] @UserId = {0}",
                                                            _context.TaskAssigned
                                                                .Where(e => e.TASKID == t.Task.TASKID && e.TASKSERIALNO == t.Task.TASKSERIALNO && e.ISDELETED == 0)
                                                                .OrderByDescending(e => e.TASKID)
                                                                .Select(e => e.WHICHDEVELOPERASSIGNED)
                                                                .FirstOrDefault()
                                                        )
                                                        .AsEnumerable()
                                                        .FirstOrDefault()?.UserName ?? "N/A",  // now safe outside EF expression
                task = t.Task
            }); ;


            var response = new { result };
            return Ok(new { response });
        }


        [HttpGet("GetAllCommentTask")]
        public IActionResult GetAllCommentTask(int UserId)
        {

            var projectList = _context.ProjectAssign.Where(e => e.USERID == UserId && e.ISDELETED == 0).ToList();
            // Get all project IDs assigned to this user
            var projectIds = projectList.Select(p => p.PROJECTID).ToList();


            // Get related tasks (ISDELETED == 0 only)
            var taskIdsWithLatestComment = _context.Comments.Where(t => t.ISDELETED == 0).Select(x => x.TASKID).ToList();

            var taskList = (from t in _context.Tasks
                            where taskIdsWithLatestComment.Contains(t.TASKID)
                            join p in _context.Projects on t.PROJECTID equals p.PROJECTID
                            join pri in _context.TASKSPRIORITIES on t.TASKSPRIORITYID equals pri.TASKSPRIORITYID
                            join cat in _context.TASKCATEGORIES on t.TASKCATEGORYID equals cat.TASKCATEGORYID
                            where projectIds.Contains(t.PROJECTID)
                            select new
                            {
                                Task = t,
                                ProjectName = p.PROJECTNAME,
                                PriorityDescription = pri.DESCRIPTION,
                                CategoryName = cat.NAME,
                                CategoryDesc = cat.DESCRIPTION
                            }).OrderByDescending(x => x.Task.TASKID).ToList(); // ⬅ break the expression tree here





            var result = taskList.Select(t => new
            {
                MainId = t.Task.TASKID,

                SenderEmail = _context.TaskEmailS
                                                        .Where(e => e.TASKID == t.Task.TASKID && e.TASKSERIALNO == t.Task.TASKSERIALNO)
                                                        .OrderBy(e => e.TASKID)
                                                        .Select(e => e.SENDEREMAILADDRESS)
                                                        .FirstOrDefault(),



                Status = _context.STATUS
                                                        .Where(s => s.STATUSID ==
                                                            _context.TaskStatuses
                                                                .Where(e => e.TASKID == t.Task.TASKID && e.TASKSERIALNO == t.Task.TASKSERIALNO && e.ISDELETED == 0)
                                                                .OrderByDescending(e => e.TASKID)
                                                                .Select(e => e.STATUSID)
                                                                .FirstOrDefault()
                                                        )
                                                        .Select(s => s.NAME)
                                                        .FirstOrDefault(),
                StatusId = _context.TaskStatuses
                                                        .Where(e => e.TASKID == t.Task.TASKID && e.TASKSERIALNO == t.Task.TASKSERIALNO && e.ISDELETED == 0)
                                                        .OrderByDescending(e => e.TASKID)
                                                        .Select(e => e.STATUSID)
                                                        .FirstOrDefault(),
                CategoryName = t.CategoryName,
                ProjectName = t.ProjectName,
                PriorityDescription = t.PriorityDescription,
                category = t.CategoryDesc,
                priority = t.PriorityDescription,
                assign = _context.Set<UserNameResultDTO>()
                                                        .FromSqlRaw(
                                                            "EXEC [TMS].[USP_GetUserName] @UserId = {0}",
                                                            _context.TaskAssigned
                                                                .Where(e => e.TASKID == t.Task.TASKID && e.TASKSERIALNO == t.Task.TASKSERIALNO && e.ISDELETED == 0)
                                                                .OrderByDescending(e => e.TASKID)
                                                                .Select(e => e.WHICHDEVELOPERASSIGNED)
                                                                .FirstOrDefault()
                                                        )
                                                        .AsEnumerable()
                                                        .FirstOrDefault()?.UserName ?? "N/A",  // now safe outside EF expression
                task = t.Task
            }); ;


            var response = new { result };
            return Ok(new { response });
        }
         

        [HttpPost("NewTask")]
        public IActionResult NewTask([FromBody] Tassk tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            tsk.ISDELETED = 0;
            tsk.TASKID = null;
            _context.Tasks.Add(tsk);
            _context.SaveChanges();

            TasskStatus tskst = new TasskStatus();
            tskst.TASKSERIALNO           = tsk.TASKSERIALNO;
            tskst.TASKID                = _context.Tasks.Where(t => t.TASKSERIALNO == tsk.TASKSERIALNO).Select(t => t.TASKID).FirstOrDefault();
            tskst.STATUSID               = 1;
            tskst.DESCRIPTION            = tsk.DESCRIPTION;
            tskst.TASKCATEGORYID         = tsk.TASKCATEGORYID;
            tskst.TASKPRIORITYID         = tsk.TASKSPRIORITYID;
            tskst.ISDELETED              = 0;
            tskst.CREATEDBY              = tsk.CREATEDBY;
            tskst.CREATEDON              = tsk.CREATEDON;
            tskst.UPDATEDBY              = tsk.UPDATEDBY;
            tskst.UPDATEDON              = tsk.UPDATEDON;
            _context.TaskStatuses.Add(tskst);
            _context.SaveChanges();

            var result = _context.Set<UserEmailResultDTO>()
                   .FromSqlRaw("EXEC [TMS].[USP_GetUserEmail] @UserId = {0}", tsk.CREATEDBY)
                   .ToList();

            if (result == null || string.IsNullOrEmpty(result.SingleOrDefault().UserEmail))
            {
                return BadRequest(new { message = "Unable to retrieve user email." });
            }

            TaskEmail tskemail = new TaskEmail();
            tskemail.TASKID             = tsk.TASKID;
            tskemail.TASKSERIALNO       = tsk.TASKSERIALNO; 
            tskemail.SUBJECT            = tsk.DESCRIPTION; 
            tskemail.BODYCONTENTTYPE    = "html";
            tskemail.BODYCONTENT        = @"<html><head><meta http-equiv=\'Content-Type\' content=\'text/html; charset=utf-8\'></head> <body> <div id=\'ManualEntry\' class=\'3\' style=\'\'>  <p><b>This task enter Manually</b></p>  <div>  </body> </html>";
            tskemail.SENDEREMAILADDRESS = result.SingleOrDefault().UserEmail;
            tskemail.FROMEMAILADDRESS   = result.SingleOrDefault().UserEmail; 
            tskemail.ISDELETED          = 0;
            tskemail.CREATEDBY          = tsk.CREATEDBY;
            tskemail.CREATEDON          = tsk.CREATEDON;
            tskemail.UPDATEDBY          = tsk.UPDATEDBY;
            tskemail.UPDATEDON          = tsk.UPDATEDON;
            _context.TaskEmailS.Add(tskemail);
            _context.SaveChanges();


            return Ok(new { message = "Task Successfully Added!!! " });
        }

        [HttpPatch("UpdTask")]
        public IActionResult UpdTask([FromBody] Tassk tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Task = _context.Tasks.Find(tsk.TASKID);

            if (_Task != null)
            {
                _context.Entry(_Task).State = EntityState.Detached;
                // Step 2: Modify the Task property
                _Task.PROJECTID         = tsk.PROJECTID;
                _Task.DESCRIPTION       = tsk.DESCRIPTION;
                _Task.TASKSPRIORITYID   = tsk.TASKSPRIORITYID;
                _Task.TASKCATEGORYID    = tsk.TASKCATEGORYID;
                _Task.DUEDATE           = tsk.DUEDATE;
                _Task.USERID            = tsk.USERID;
                _Task.ISDELETED         = 0;
                _Task.CREATEDBY         = tsk.CREATEDBY;
                _Task.CREATEDON         = tsk.CREATEDON;
                _Task.UPDATEDBY         = tsk.UPDATEDBY;
                _Task.UPDATEDON         = tsk.UPDATEDON;

                // Step 3: Save the changes to the database
                _context.Tasks.Update(_Task);
                _context.SaveChangesAsync();

                return Ok(new { message = "Task Successfully Modified!!!" + tsk.TASKID });
            }
            else
            {
                // Handle case when the Project is not found
                return Ok(new { message = "Task not found!!!" });
            }
        }

        [HttpPatch("DelTask")]
        public IActionResult DelTask([FromBody] Tassk tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Task = _context.Tasks.Find(tsk.TASKID);

            if (_Task != null)
            {

                _context.Entry(_Task).State = EntityState.Detached;
                // Step 2: Modify the Project property
                _Task.ISDELETED =0;
                _Task.CREATEDBY = tsk.CREATEDBY;
                _Task.CREATEDON = tsk.CREATEDON;
                _Task.UPDATEDBY = tsk.UPDATEDBY;
                _Task.UPDATEDON = tsk.UPDATEDON;

                // Step 3: Save the changes to the database
                _context.Tasks.Update(_Task);
                _context.SaveChangesAsync();

                return Ok(new { message = "Task Successfully Deleted!!! " + tsk.TASKID});
            }
            else
            {
                // Handle case when the Project is not found
                return Ok(new { message = "Task not found!!!" });
            }
        }
        #endregion
        #region tis is related to fetch all developer and customer list

        [HttpGet("TaskEmail")]
        public IActionResult TaskEmail(int TaskId)
        {
            if (TaskId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var  result  = _context.TaskEmailS
                .Where(te  => te.TASKID == TaskId)
                .Select(te => te.BODYCONTENT)
                .ToList();
 
            string concatenated = string.Join("<br>", result);

            var response = new { concatenated };
            return Ok(new { response });
        }

        [HttpGet("TaskEmailAttachment")]
        public IActionResult TaskEmailAttachment(int TaskId)
        {
            if (TaskId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var result = _context.TaskEmailAttachment
                 .Where(t => t.TASKID == TaskId)
                 .Select(t => t.URL)
                 .ToList(); 

            var response = new { result };
            return Ok(new { response });
        }


        [HttpGet("TaskDocAttachment")]
        public IActionResult TaskDocAttachment(int TaskId)
        {
            if (TaskId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var result = _context.Documents
                 .Where(t => t.TASKID == TaskId)
                 .Select(t => t.DOCUMENTURL)
                 .ToList();

            var response = new { result };
            return Ok(new { response });
        }



        [HttpGet("DownloadDoc")]
        public IActionResult DownloadDoc(string filename)
        {
            string filePath = System.IO.Path.Combine(_mailAttachmentPath, filename);
            if (string.IsNullOrWhiteSpace(filename))
            {
                return BadRequest(new { message = "Invalid file path!" });
            }  
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { message = "File not found." });
            }

            var fileName = System.IO.Path.GetFileName(filePath);
            var fileBytes = System.IO.File.ReadAllBytes(filePath); 
            // Let browser download the file with correct name
            return File(fileBytes, "application/octet-stream", filename);

        }










        [HttpGet("ChangeTaskStatus")]
        public IActionResult ChangeTaskStatus(int TaskId,int statusId,int doneby)
        {
            if (TaskId<=0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Task = _context.Tasks.Find(TaskId);


            // Step 2: Update all previous TaskStatus rows for this TASKID
            var existingStatuses = _context.TaskStatuses
                .Where(ts => ts.TASKID == _Task.TASKID && ts.ISDELETED == 0)
                .ToList();

            foreach (var status in existingStatuses)
            {
                status.ISDELETED = 1; // Mark existing statuses as deleted
                status.UPDATEDBY = doneby;
                status.UPDATEDON = DateTime.Now;
                status.CREATEDBY = doneby;
                status.CREATEDON = DateTime.Now;
                status.UPDATEDBY = doneby;
                status.UPDATEDON = DateTime.Now;
            }


            var tsk = new TasskStatus
            {
                TASKID              = _Task.TASKID,
                TASKSERIALNO        = _Task.TASKSERIALNO,
                STATUSID            = statusId,
                TASKCATEGORYID      = _Task.TASKCATEGORYID,
                TASKPRIORITYID      =  _Task.TASKSPRIORITYID,
                DESCRIPTION         = _Task.DESCRIPTION,
                ISDELETED           = 0,
                CREATEDBY           = doneby,
                CREATEDON           = DateTime.Now,
                UPDATEDBY           = doneby,
                UPDATEDON           = DateTime.Now
            }; 
            _context.TaskStatuses.Add(tsk); 
            _context.SaveChanges();

            return Ok(new { message = "Task Status Successfully Modified!!!" + tsk.TASKID });
            
             
        }


        [HttpGet("TaskStatusList")]
        public IActionResult TaskStatusList(int TaskId)
        {
            if (TaskId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var result = (from tsks in _context.TaskStatuses
                          join sts in _context.STATUS on tsks.STATUSID equals sts.STATUSID
                          where tsks.TASKID == TaskId
                          orderby tsks.CREATEDON descending
                          select new { sts.NAME, tsks.CREATEDON })
                   .AsEnumerable() // switch to in-memory after data is fetched from DB
                   .Select(x => x.NAME + " : Created on " + (x.CREATEDON?.ToString("dd/MMM/yyyy HH:mm") ?? ""))
                   .ToList();
            var response = new { result };
            return Ok(new { response });
        }




      
        [HttpGet("TaskAssignmentList")]
        public IActionResult TaskAssignmentList(int TaskId)
        {
            if (TaskId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var result = _context.Set<AssignmentDetailDto>()
                      .FromSqlRaw("EXEC [TMS].[USP_GetAssignedOperationDetails] @TaskId = {0}", TaskId)
                      .ToList();

            var response = new { result };
            return Ok(new { response });
        }

         

        [HttpGet("UpdTaskAssignment")]
        public async Task<IActionResult> UpdTaskAssignment(int TaskId, string whoAssign, int ToWhomassign)
        {
            if (TaskId <= 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
                var userId = _context.Set<UserIdResultDTO>()
                              .FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", whoAssign)
                              .AsEnumerable()
                              .FirstOrDefault()?.UserId ?? 0;

            // Step 1: Retrieve the task
            var _Task = await _context.Tasks.FindAsync(TaskId);
            if (_Task == null)
            {
                return NotFound(new { message = "Task not found." });
            }

            // Step 2: Get active assignments
            var result = await _context.TaskAssigned
                .Where(ao => ao.TASKID == TaskId && ao.ISDELETED == 0)
                .ToListAsync();

            if (result == null || result.Count == 0)
            {
                // No active assignment, add new
                var tsk = new TaskAssign
                {
                    TASKID = _Task.TASKID,
                    TASKSERIALNO = _Task.TASKSERIALNO,
                    WHOASSIGNED = userId,
                    WHICHDEVELOPERASSIGNED = ToWhomassign,
                    ISDELETED = 0,
                    CREATEDBY = userId,
                    CREATEDON = DateTime.Now,
                    UPDATEDBY = userId,
                    UPDATEDON = DateTime.Now
                };

                _context.TaskAssigned.Add(tsk);
                await _context.SaveChangesAsync();




            }
            else
            {
                // Mark previous active assignments as deleted
                foreach (var item in result)
                {
                    item.ISDELETED = userId;
                    item.UPDATEDBY = userId;
                    item.UPDATEDON = DateTime.Now;
                }
                _context.TaskAssigned.UpdateRange(result);
                await _context.SaveChangesAsync();

                // Add new assignment
                var newTaskAssign = new TaskAssign
                {
                    TASKID = _Task.TASKID,
                    TASKSERIALNO = _Task.TASKSERIALNO,
                    WHOASSIGNED = userId,
                    WHICHDEVELOPERASSIGNED = ToWhomassign,
                    ISDELETED = 0,
                    CREATEDBY = userId,
                    CREATEDON = DateTime.Now,
                    UPDATEDBY = userId,
                    UPDATEDON = DateTime.Now
                };

                _context.TaskAssigned.Add(newTaskAssign);
                await _context.SaveChangesAsync(); 
            }

            return Ok(new { message = $"Task assignment successfully done! Task ID: {TaskId}" });
        }
         

        [HttpGet("AddTaskComment")]
        public IActionResult AddTaskComment(int TaskId, string who,string Comment)
        {
            if (TaskId <= 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var userId = _context.Set<UserIdResultDTO>()
                .FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", who)
                .AsEnumerable()
                .FirstOrDefault()?.UserId ?? 0;

            // Step 1: Retrieve the Project from the database
            var _Task = _context.Tasks.Find(TaskId); ;
            if (_Task != null)
            {
                var tsk = new Comment
                {
                    TASKID = _Task.TASKID,
                    TASKSERIALNO =_Task.TASKSERIALNO,
                    COMMENTTEXT= Comment,
                    ISDELETED = 0,
                    CREATEDBY = userId,
                    CREATEDON = DateTime.Now,
                    UPDATEDBY = userId,
                    UPDATEDON = DateTime.Now
                };
                _context.Comments.Add(tsk);
                _context.SaveChanges();

            }


            return Ok(new { message = "Task comment added Successfully Done!!!" + TaskId });
        }



        [HttpGet("TaskCommentList")]
        public IActionResult TaskCommentList(int TaskId)
        {
            if (TaskId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var result = _context.Set<AssignmentDetailDto>()
                      .FromSqlRaw("EXEC [TMS].[USP_GetCommentOperationDetails] @TaskId = {0}", TaskId)
                      .ToList();

            var response = new { result };
            return Ok(new { response });
        }

         
        [HttpGet("DeleteTask")]
        public async Task<IActionResult> TaskCommentList(int TaskId, int userId)
        {
            if (TaskId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Task from the database
            var _Task = await _context.Tasks.FindAsync(TaskId);

            if (_Task != null)
            {
                // Detach the entity
                _context.Entry(_Task).State = EntityState.Detached;

                //--=============== Modify the Task entity
                //_Task.ISDELETED = 1;
                //_Task.CREATEDBY = userId;
                //_Task.CREATEDON = DateTime.Now;
                //_Task.UPDATEDBY = userId;
                //_Task.UPDATEDON = DateTime.Now;

                //--=============== Update and save changes
                //_context.Tasks.Update(_Task);
                //await _context.SaveChangesAsync();  // ✅ await here

                //--=============== Retrieve TaskStatus
                //var _TaskStatus = await _context.TaskStatuses.FindAsync(TaskId);
                //if (_TaskStatus != null)
                //{
                //    _context.Entry(_TaskStatus).State = EntityState.Detached;

                //    _TaskStatus.ISDELETED = 1;
                //    _TaskStatus.CREATEDBY = userId;
                //    _TaskStatus.CREATEDON = DateTime.Now;
                //    _TaskStatus.UPDATEDBY = userId;
                //    _TaskStatus.UPDATEDON = DateTime.Now;

                //    _context.TaskStatuses.Update(_TaskStatus);
                //    await _context.SaveChangesAsync();  // ✅ await here too
                //}
                //var taskStatuses =  await _context.TaskStatuses
                //                    .Where(ts => ts.TASKID == TaskId)
                //                    .ToListAsync();

                var taskStatus = await _context.TaskStatuses
                            .Where(ts => ts.TASKID == TaskId)
                            .OrderByDescending(ts => ts.TASKSSTATUSID)
                            .FirstOrDefaultAsync();

                if (taskStatus != null)
                {

                    taskStatus.STATUSID = 4;
                    taskStatus.CREATEDBY = userId;
                    taskStatus.CREATEDON = DateTime.Now;
                    taskStatus.UPDATEDBY = userId;
                    taskStatus.UPDATEDON = DateTime.Now;

                    await _context.SaveChangesAsync();
                }
                //foreach (var status in taskStatuses)
                //{
                //    status.ISDELETED = 1;
                //    status.CREATEDBY = userId;
                //    status.CREATEDON = DateTime.Now;
                //    status.UPDATEDBY = userId;
                //    status.UPDATEDON = DateTime.Now;

                //    // No need to call Update explicitly for tracked entities
                //} 
                //// Save all changes at once
                //await _context.SaveChangesAsync();

                return Ok(new { message = "Task Successfully Deleted!!! " + TaskId });
            }
            else
            {
                return Ok(new { message = "Task not found!!!" });
            }
        }



        [HttpGet("TaskDocumentList")]
        public IActionResult TaskDocumentList(int TaskId)
        {
            if (TaskId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var result = _context.Set<AssignmentDetailDto>()
                      .FromSqlRaw("EXEC [TMS].[USP_GetDocumentOperationDetails] @TaskId = {0}", TaskId)
                      .ToList();

            var response = new { result };
            return Ok(new { response });
        }




        [HttpGet("GetCustomer")]
        public IActionResult GetCustomer(string Type)
        {
            if (Type == " ")
            {
                return BadRequest(new { message = "Invalid Type request!!!" });
            }
            var result = _context.Set<AssignmentDetailDto>()
                      .FromSqlRaw("EXEC [TMS].[USP_GetCustomerContact] @Type = {0}", Type)
                      .ToList();

            var response = new { result };
            return Ok(new { response });
        }

        #endregion
        #region For Dashboard
        [HttpGet("DashboardPart1")]
        public IActionResult DashboardPart1(string UserId)
        { 
            var result = _context.Set<DashboardPart1tDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_DashboardPart1] @UserId = {0}", UserId)
                      .ToList();

            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("DashboardPart2")]
        public IActionResult DashboardPart2(string UserId)
        {
            var result = _context.Set<DashboardPart1TO4tDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_DashboardPart2]@UserId = {0}", UserId)
                      .ToList();

            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("DashboardPart3")]
        public IActionResult DashboardPart3(string UserId)
        {
            var result = _context.Set<DashboardPart1TO4tDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_DashboardPart3] @UserId = {0}", UserId)
                      .ToList();

            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("DashboardPart4")]
        public IActionResult DashboardPart4(string UserId)
        {
            var result = _context.Set<DashboardPart1TO4tDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_DashboardPart4] @UserId = {0}", UserId)
                      .ToList();

            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("DashboardPart5")]
        public IActionResult DashboardPart5(string UserId)
        {
            var result = _context.Set<DashboardPart5DTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_DashboardPart5] @UserId = {0}", UserId)
                      .ToList();

            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("DashboardPart6")]
        public IActionResult DashboardPart6(string UserId)
        {
            var result = _context.Set<DashboardPart6DTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_DashboardPart6] @UserId = {0}", UserId)
                      .ToList();

            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("DashboardPart7")]
        public IActionResult DashboardPart7(string UserId)
        {
            var result = _context.Set<DashboardPart7DTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_DashboardPart7] @UserId = {0}", UserId)
                      .ToList();

            var response = new { result };
            return Ok(new { response });
        }





        [HttpGet("Downloadtemplate")]
        public IActionResult Downloadtemplate(string filename)
        {
            string filePath = System.IO.Path.Combine(_TaskTemplatePath, filename);
            if (string.IsNullOrWhiteSpace(filename))
            {
                return BadRequest(new { message = "Invalid file path!" });
            }
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { message = "File not found." });
            }

            var fileName = System.IO.Path.GetFileName(filePath);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            // Let browser download the file with correct name
            return File(fileBytes, "application/octet-stream", filename);

        }
        #endregion
        #region For Analytics
        [HttpGet("AnalyticsPart1")]
        public IActionResult AnalyticsPart1(string Type, string val, string UserId)
        {

            var result = _context.Set<AnalyticsPart1tDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_AnalyticsPart1] @Type = {0}, @val = {1}, @UserId = {2}", Type, val, UserId); 
            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("AnalyticsPart2")]
        public IActionResult AnalyticsPart2(string Type, string val, string UserId)
        {

            var result = _context.Set<AnalyticsPart2tDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_AnalyticsPart2] @Type = {0}, @val = {1},@UserId = {2}", Type, val, UserId);
            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("AnalyticsPart3")]
        public IActionResult AnalyticsPart3(string Type, string val, string UserId)
        {

            var result = _context.Set<AnalyticsPart3tDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_AnalyticsPart3] @Type = {0}, @val = {1} ,@UserId = {2}", Type, val, UserId);
            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("AnalyticsPart4")]
        public IActionResult AnalyticsPart4(string Type, string val, string UserId)
        {

            var result = _context.Set<AnalyticsPart3tDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_AnalyticsPart4] @Type = {0}, @val = {1}, @UserId = {2}", Type, val, UserId);
            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("AnalyticsPart5")]
        public IActionResult AnalyticsPart5(string Type, string val, string UserId)
        {

            var result = _context.Set<AnalyticsPart5tDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_AnalyticsPart5] @Type = {0}, @val = {1},@UserId = {2}", Type, val, UserId);
            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("AnalyticsPart6")]
        public IActionResult AnalyticsPart6(string Type, string val, string UserId)
        { 
            var result = _context.Set<AnalyticsPart3tDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_AnalyticsPart6] @Type = {0}, @val = {1},@UserId = {2}", Type, val, UserId);
            var response = new { result };
            return Ok(new { response });
        }

        [HttpGet("AnalyticsPart7")]
        public IActionResult AnalyticsPart7(string Type, string val, string UserId)
        {

            var result = _context.Set<AnalyticsPart1tDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_AnalyticsPart7] @Type = {0}, @val = {1},@UserId = {2}", Type, val, UserId);
            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("AnalyticsPart8")]
        public IActionResult AnalyticsPart8(string Type, string val, string UserId)
        {

            var result = _context.Set<AnalyticsPart1tDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_AnalyticsPart8] @Type = {0}, @val = {1},@UserId = {2}", Type, val, UserId);
            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("AnalyticsPart9")]
        public IActionResult AnalyticsPart9(string Type, string val, string UserId)
        {

            var result = _context.Set<AnalyticsPart1tDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_AnalyticsPart9] @Type = {0}, @val = {1},@UserId = {2}", Type, val, UserId);
            var response = new { result };
            return Ok(new { response });
        }
        [HttpGet("AnalyticsPart10")]
        public IActionResult AnalyticsPart10(string Type, string val, string UserId)
        {
            var result = _context.Set<AnalyticsPart1tDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_AnalyticsPart10] @Type = {0}, @val = {1},@UserId = {2}", Type, val, UserId);
            var response = new { result };
            return Ok(new { response });
        }
        #endregion
        #region HQ Page
        [HttpGet("HQ")]
        public IActionResult HQ(string UserId)
        {

            var result = _context.Set<HQDTO>()
                      .FromSqlRaw("EXEC [TMS].[USP_HQ] @UserId = {0}", UserId);
            var response = new { result };
            return Ok(new { response });
        }
        #endregion
        #region  TEAMFEED
        [HttpGet("TeamFeed")]
        public IActionResult TeamFeed(string UserId)
        {

            var result = _context.Set<TeamFeed>()
                      .FromSqlRaw("EXEC [TMS].[USP_TEAMFEED] @UserId = {0}", UserId);
            var response = new { result };
            return Ok(new { response });
        }
        #endregion
        #region  Agentqueue
        [HttpGet("Agentqueue")]
        public IActionResult Agentqueue(string UserId)
        {

            var result = _context.Set<TeamFeed>()
                      .FromSqlRaw("EXEC [TMS].[USP_AGENTQUEUE] @UserId = {0}", UserId);
            var response = new { result };
            return Ok(new { response });
        }
        #endregion
        #region  Agentqueue
        [HttpGet("Teamqueue")]
        public IActionResult Teamqueue(string UserId)
        {

            var result = _context.Set<TeamFeed>()
                      .FromSqlRaw("EXEC [TMS].[USP_TEAMQUEUE] @UserId = {0}", UserId);
            var response = new { result };
            return Ok(new { response });
        }
        #endregion


        #region  ProjectAssign & Update
        [HttpGet("ProjectAssign")]
        public IActionResult ProjectAssign()
        { 
            var result = _context.Set<TeamFeed>()
                      .FromSqlRaw("EXEC [TMS].[USP_PROJASSIGNLIST]");
            var response = new { result };
            return Ok(new { response });
        }

        [HttpGet("ProjectAssignUpdate")]
        public IActionResult ProjectAssignUpdate(string Developer, string ProjDetail, int userId)
        {
            var result = _context.Set<TeamFeed>()
                           .FromSqlRaw("EXEC [TMS].[USP_PROJASSIGNUPD] @UserId={0}, @Developer={1}, @ProjDetail={2}", userId, Developer, ProjDetail);
            var response = new { result };
            return Ok(new { response });
        }
        #endregion
    }
}

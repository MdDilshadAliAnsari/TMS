using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TMS.Services.Model; 
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Data.SqlClient;
using System.IO.Compression;
using System.Xml.Linq;
using System.Data;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using DocumentFormat.OpenXml.Bibliography;

namespace TMS.Services.Controllers
{
    [ApiController]
    [Route("api/v{1:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class DocumentController : ControllerBase
    {
        private static long lastNumber = 0;
        private static readonly object lockObj = new object();
        #region  Variable Constructor and Dependency injection in Constructor level
        private readonly TMSDbContext _context;
        private readonly IWebHostEnvironment _env;
        public DocumentController(TMSDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        #endregion
        #region Operation on Tasks
        [HttpGet("GetAllDocument")]
        public IActionResult GetDocument(int TaskId)
        {
            if (TaskId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var result = _context.Documents.Where(m => m.TASKID == TaskId && m.ISDELETED==0).ToList();
            if (result == null)
            {
                // Handle case when the Document is not found
                return Ok(new { message = "Document not found!!!" });
            }
            var response = new { result };
            return Ok(new { response });
        }
        
        [HttpGet("GetDocument")]
        public IActionResult GetDocument(int TaskId, int DocumentId)
        {
            if (TaskId == 0)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            var result = _context.Documents.Where(m => m.TASKID == TaskId && m.DOCUMENTID == DocumentId && m.ISDELETED == 0).ToList();
            if (result == null)
            {
                return Ok(new { message = "Document not found!!!" });
            }
            var response = new { result };
            return Ok(new { response });
        }

        [HttpPost("NewDocument")]
        public IActionResult NewDocument([FromBody] Document tsk)
        {
            
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }
            tsk.DOCUMENTID = null;
            tsk.DOCUMENTURL= PathEscaper.Unescape(tsk.DOCUMENTURL).ToString();
            tsk.ISDELETED = 0;
            _context.Documents.Add(tsk);
            _context.SaveChanges();
            return Ok(new { message = "Document Successfully Added!!! " });
        }

        [HttpPatch("UpdDocument")]
        public IActionResult UpdDocument([FromBody] Document tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Task = _context.Documents.Find(tsk.DOCUMENTID);

            if (_Task != null)
            {
                _context.Entry(_Task).State = EntityState.Detached;
                // Step 2: Modify the Task property
                _Task.PROJECTID     = tsk.PROJECTID;
                _Task.TASKID        = tsk.TASKID;
                _Task.TASKSSTATUSID = tsk.TASKSSTATUSID;
                _Task.DOCUMENTURL   = tsk.DOCUMENTURL;
                _Task.ISDELETED     = 0;
                _Task.CREATEDBY     = tsk.CREATEDBY;
                _Task.CREATEDON     = tsk.CREATEDON;
                _Task.UPDATEDBY     = tsk.UPDATEDBY;
                _Task.UPDATEDON     = tsk.UPDATEDON;

                // Step 3: Save the changes to the database
                _context.Documents.Update(_Task);
                _context.SaveChangesAsync();

                return Ok(new { message = "Document Successfully Modified!!!" + tsk.DOCUMENTID });
            }
            else
            {
                // Handle case when the Document is not found
                return Ok(new { message = "Document not found!!!" });
            }
        }

        [HttpPatch("DelDocument")]
        public IActionResult DelDocument([FromBody] Document tsk)
        {
            if (tsk is null)
            {
                return BadRequest(new { message = "Invalid user request!!!" });
            }

            // Step 1: Retrieve the Project from the database
            var _Task = _context.Documents.Find(tsk.DOCUMENTID);

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
                _context.Documents.Update(_Task);
                _context.SaveChangesAsync();

                return Ok(new { message = "Documents Successfully Deleted!!! " + tsk.DOCUMENTID });
            }
            else
            {
                // Handle case when the Document is not found
                return Ok(new { message = "Document not found!!!" });
            }
        }
        #endregion
        # region upload document for respective task
        [HttpPost("uploadDoc")]
        public async Task<IActionResult> uploadDoc(int userId, int taskId,IFormFile file)
        { 
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file uploaded." });

            try
            {
                var uploadsPath = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var extension = Path.GetExtension(file.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{uniqueFileName}";
                
                var task = await _context.Tasks.FindAsync(taskId);
                if (task == null)
                    return NotFound(new { message = "Task not found." });

                var latestStatusId = await _context.TaskStatuses
                .Where(e => e.TASKID == task.TASKID && e.TASKSERIALNO == task.TASKSERIALNO && e.ISDELETED == 0)
                .OrderByDescending(e => e.CREATEDON)
                .Select(e => e.STATUSID)
                .FirstOrDefaultAsync();

                var doc = new Document
                {
                    PROJECTID = task.PROJECTID,
                    TASKID = task.TASKID,
                    TASKSERIALNO = task.TASKSERIALNO,
                    TASKSSTATUSID = latestStatusId,
                    DOCUMENTURL = fileUrl,
                    ISDELETED = 0,
                    CREATEDBY = userId,
                    CREATEDON = DateTime.Now,
                    UPDATEDBY = userId,
                    UPDATEDON = DateTime.Now
                };

                _context.Documents.Add(doc);
                await _context.SaveChangesAsync(); 
                return Ok(new
                {
                    message = "File uploaded successfully.",
                    fileUrl,
                    fileName = file.FileName,
                    uploadedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "File upload failed.", error = ex.Message });
            }
        }


        [HttpPost("uploadTaskDoc")]
        public async Task<IActionResult> uploadTaskDoc(int userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file uploaded." });

            try
            {
                var uploadsPath = Path.Combine(_env.WebRootPath ?? "wwwroot", "BulkTask");
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var extension = Path.GetExtension(file.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{uniqueFileName}"; 
                DataTable dt = ReadExcelToDataTable(filePath, "Sheet1");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int i = 1;
                        
                        var ProjectId       = _context.Projects.Where(t => t.PROJECTNAME== row["Project"].ToString()).Select(t => t.PROJECTID).FirstOrDefault();
                        var ProjectName     = _context.Projects.Where(t => t.PROJECTNAME == row["Project"].ToString()).Select(t => t.PROJECTNAME).FirstOrDefault();
                        string initials     = ProjectName != null && ProjectName.Length >= 4 ? ProjectName.Substring(0, 4): ProjectName; // return shorter st
                        var CatId       = _context.TASKCATEGORIES.Where(t => t.NAME == row["Category"].ToString()).Select(t => t.TASKCATEGORYID).FirstOrDefault();
                        var PriorityID  = _context.TASKSPRIORITIES.Where(t => t.NAME == row["Priority"].ToString()).Select(t => t.TASKSPRIORITYID).FirstOrDefault();
                        var DueDate = row["DueDate"].ToString();
                        if (DateTime.TryParseExact(
                            DueDate,
                            new[] { "MM-dd-yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss" },
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateTime date))
                            {
                                
                            }

                        var CreateDate = row["CreateDate"].ToString();
                        if (DateTime.TryParseExact(
                            CreateDate,
                            new[] { "MM-dd-yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss" },
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateTime CrtDate))
                        {

                        }
                        var tsk = new Tassk()
                             {
                            TASKID              = null, 
                            //TASKSERIALNO        = CrtDate.ToString("yyyyMMddHHmmss"),// DateTime.Now.ToString("yyyyMMddHHmmss"),
                            TASKSERIALNO        = initials + " : "+GenerateUnique14DigitNumber(),
                            DESCRIPTION         = row["TaskName"].ToString(),
                            PROJECTID           = ProjectId,
                            TASKSPRIORITYID     = PriorityID,
                            TASKCATEGORYID      = CatId,
                            DUEDATE             = date,
                            ISDELETED = 0,
                            CREATEDBY = userId,
                            CREATEDON = CrtDate,
                            UPDATEDBY = userId,
                            UPDATEDON = CrtDate,  
                             };
                        _context.Tasks.Add(tsk);
                        _context.SaveChanges();

                        var lastTask = _context.Tasks
                                        .Where(t => t.CREATEDBY == userId && t.DESCRIPTION == row["TaskName"].ToString() && t.PROJECTID == ProjectId && t.TASKCATEGORYID == CatId && t.TASKSPRIORITYID == PriorityID)
                                        .OrderByDescending(t => t.TASKID)
                                        .FirstOrDefault();

                        TasskStatus tskst = new TasskStatus();
                        tskst.TASKSERIALNO = lastTask.TASKSERIALNO;
                        tskst.TASKID = lastTask.TASKID;
                        tskst.STATUSID = 1;
                        tskst.DESCRIPTION = lastTask.DESCRIPTION;
                        tskst.TASKCATEGORYID = lastTask.TASKCATEGORYID;
                        tskst.TASKPRIORITYID = lastTask.TASKSPRIORITYID;
                        tskst.ISDELETED = 0;
                        tskst.CREATEDBY = lastTask.CREATEDBY;
                        tskst.CREATEDON = lastTask.CREATEDON;
                        tskst.UPDATEDBY = lastTask.UPDATEDBY;
                        tskst.UPDATEDON = lastTask.UPDATEDON;
                        _context.TaskStatuses.Add(tskst);
                        _context.SaveChanges();
                        var result = _context.Set<UserEmailResultDTO>()
                               .FromSqlRaw("EXEC [TMS].[USP_GetUserEmail] @UserId = {0}", lastTask.CREATEDBY)
                               .ToList();

                        if (result == null || string.IsNullOrEmpty(result.SingleOrDefault().UserEmail))
                        {
                            return BadRequest(new { message = "Unable to retrieve user email." });
                        }

                        TaskEmail tskemail = new TaskEmail();
                        tskemail.TASKID = lastTask.TASKID;
                        tskemail.TASKSERIALNO = lastTask.TASKSERIALNO;
                        tskemail.SUBJECT = lastTask.DESCRIPTION;
                        tskemail.BODYCONTENTTYPE = "html";
                        tskemail.BODYCONTENT = @"<html><head><meta http-equiv=\'Content-Type\' content=\'text/html; charset=utf-8\'></head> <body> <div id=\'ManualEntry\' class=\'3\' style=\'\'>  <p><b>This task enter Manually</b></p>  <div>  </body> </html>";
                        tskemail.SENDEREMAILADDRESS = result.SingleOrDefault().UserEmail;
                        tskemail.FROMEMAILADDRESS = result.SingleOrDefault().UserEmail;
                        tskemail.ISDELETED = 0;
                        tskemail.CREATEDBY = lastTask.CREATEDBY;
                        tskemail.CREATEDON = lastTask.CREATEDON;
                        tskemail.UPDATEDBY = lastTask.UPDATEDBY;
                        tskemail.UPDATEDON = lastTask.UPDATEDON;
                        _context.TaskEmailS.Add(tskemail);
                        _context.SaveChanges();
                        i = i + 1;
                    }
                }
              

                return Ok(new
                {
                    message = "File uploaded successfully.",
                    fileUrl,
                    fileName = file.FileName,
                    uploadedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "File upload failed.", error = ex.Message });
            }
        }

        #endregion

        static DataTable ReadExcelToDataTable(string filePath, string sheetName)
        {
            DataTable dt = new DataTable();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(sheetName);
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        dt.Rows.Add();
                        int i = 0;
                        foreach (var cell in row.Cells(1, dt.Columns.Count))
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }
                }
            }

            return dt;
        }
        public static string GenerateUnique14DigitNumber()
        {
            lock (lockObj)
            {
                long timestamp = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss"));

                if (timestamp <= lastNumber)
                {
                    timestamp = lastNumber + 1; // force uniqueness
                }

                lastNumber = timestamp;
                return timestamp.ToString();
            }
        }
    }
    static class PathEscaper
    {
        static readonly string invalidChars = @"""\/?:<>*|";
        static readonly string escapeChar = "%";

        static readonly Regex escaper = new Regex(
            "[" + Regex.Escape(escapeChar + invalidChars) + "]",
            RegexOptions.Compiled);
        static readonly Regex unescaper = new Regex(
            Regex.Escape(escapeChar) + "([0-9A-Z]{4})",
            RegexOptions.Compiled);

        public static string Escape(string path)
        {
            return escaper.Replace(path,
                m => escapeChar + ((short)(m.Value[0])).ToString("X4"));
        }

        public static string Unescape(string path)
        {
            return unescaper.Replace(path,
                m => ((char)Convert.ToInt16(m.Groups[1].Value, 16)).ToString());
        }




    }
    
}

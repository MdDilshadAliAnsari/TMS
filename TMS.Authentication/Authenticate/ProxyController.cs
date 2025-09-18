using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using TMS.Authentication.Model;
using TMS.Domain;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TMS.Authentication.Authenticate
{
    [Authorize]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly TMSDbContext _context;
        public ProxyController(IHttpClientFactory httpClientFactory, TMSDbContext context)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();

        }

        #region This Section For Loger Services
        [HttpPost("Loggers")]
        [Authorize]
        public async Task<IActionResult> Logger([FromBody] ExceptionLogger log)
        { 
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:Loggers"];
            string jsonData = JsonConvert.SerializeObject(log); 
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PostToExternalService(productServiceUrl, content);
            return Content(result, "application/json");

        }
        #endregion
        #region This Section For Master data which is used for Dropdown binding
        [HttpGet("MSTPRIORITY")]
        public async Task<IActionResult> MSTPRIORITY( )
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:TASKSPRIORITYlIST"];            
            var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");

        }
        [HttpGet("TASKSTATUS")]
        public async Task<IActionResult> TASKSTATUS()
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:TASKSTATUS"];
            var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");

        }
        [HttpGet("TASKCATEGORY")]
        public async Task<IActionResult> TASKCATEGORY()
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:TASKCATEGORY"];
            var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");

        }
         
        [HttpGet("PROJECTLIST")]
        public async Task<IActionResult> PROJECTLIST()
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:ProjectList"];
            var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");

        }
        #endregion
        #region This section is used for Project Module

        [HttpGet("GetAllProj")] 
        public async Task<IActionResult> GetAllProj()
        { 
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetAllProject"];
            var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");

        }
        
        [HttpGet("GetProjByID")]
        public async Task<IActionResult> GetProjByID(int Id)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetProject"];  
            var result = await GetToExternalService(productServiceUrl+Id.ToString());
            return Content(result, "application/json");

        }
        [HttpPost("NewProject")]
        public async Task<IActionResult> NewProject([FromBody] Project Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:NewProject"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PostToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }

        [HttpPatch("UpdProject")]
        public async Task<IActionResult> UpdProject([FromBody] Project Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:UpdProject"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
        [HttpPatch("DelProject")]
        public async Task<IActionResult> DelProject([FromBody] Project Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DelProject"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
        #endregion
        #region This section is used for  Task

        [HttpGet("GetAllTask")]
        public async Task<IActionResult> GetAllTask()
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                         .AsEnumerable().FirstOrDefault()?.UserId ?? 0; 
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetAllTask"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl); 
            //var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetAllTask"];
            //var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");

        }


        [HttpGet("GetTaskByID")]
        public async Task<IActionResult> GetTaskByID(int Id)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                         .AsEnumerable().FirstOrDefault()?.UserId ?? 0;


            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetTask"];
            var formattedUrl = productServiceUrl.Replace("{TaskId}", Id.ToString()).Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl);



            //var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetTask"];
            //var result = await GetToExternalService(productServiceUrl + Id.ToString());
            return Content(result, "application/json");

        }


        [HttpGet("GetAllAssignTask")]
        public async Task<IActionResult> GetAllAssignTask()
        {

            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                         .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetAllAssignTask"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl);

            //var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetAllAssignTask"];
            //var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");

        }
        [HttpGet("GetAllCommentTask")]
        public async Task<IActionResult> GetAllCommentTask()
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                         .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetAllCommentTask"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl);

            //var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetAllCommentTask"];
            //var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");

        }









        [HttpPost("NewTask")]
        public async Task<IActionResult> NewTask([FromBody] Tassk Proj)
        {
            var username    =   User.Identity?.Name;
            var userId      =   _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var email       =    _context.USERS
                                  .Where(e => e.USERNAME == username)
                                  .Select(e => e.EMAILID)
                                  .FirstOrDefault();

            Proj.CREATEDBY  =    userId;
            Proj.UPDATEDBY  =    userId;
          
           var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:NewTask"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PostToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }

        [HttpPatch("UpdTask")]
        public async Task<IActionResult> UpdTask([FromBody] Tassk Proj)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                         .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            Proj.CREATEDBY = userId;
            Proj.UPDATEDBY = userId;

            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:UpdTask"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
        [HttpPatch("DelTask")]
        public async Task<IActionResult> DelTask([FromBody] Tassk Proj)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                         .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            Proj.CREATEDBY = userId;
            Proj.UPDATEDBY = userId;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DelTask"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }

        [HttpGet("TaskEmail")]
        public async Task<IActionResult> TaskEmail(int Id)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:TaskEmail"];
            var result = await GetToExternalService(productServiceUrl + Id.ToString());
            return Content(result, "application/json");
        }


        [HttpGet("TaskEmailAttachment")]
        public async Task<IActionResult> TaskEmailAttachment(int Id)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:TaskEmailAttachment"];
            var result = await GetToExternalService(productServiceUrl + Id.ToString());
            return Content(result, "application/json");
        }


        [HttpGet("TaskDocAttachment")]
        public async Task<IActionResult> TaskDocAttachment(int Id)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:TaskDocAttachment"];
            var result = await GetToExternalService(productServiceUrl + Id.ToString());
            return Content(result, "application/json");
        }

        [HttpGet("DownloadDoc")]
        public async Task<IActionResult> DownloadDoc(string filename)
        { 
           // var downloadUrlBase = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DownloadDoc"];  
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DownloadDoc"];
            var result = await GetToExternalServiceforDownload(productServiceUrl , filename.ToString());  
            return File(result, "application/octet-stream", filename);
        }


        [HttpGet("ChangeTaskStatus")]
        public async Task<IActionResult> ChangeTaskStatus(int Id, int statusId)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                         .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:ChangeTaskStatus"]; 
            var formattedUrl = productServiceUrl.Replace("{TaskId}", Id.ToString()).Replace("{statusId}", statusId.ToString()).Replace("{doneby}", userId.ToString()); 
            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");
        }


        [HttpGet("TaskStatusList")]
        public async Task<IActionResult> TaskStatusList(int Id)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:TaskStatusList"];
            var result = await GetToExternalService(productServiceUrl + Id.ToString());
            return Content(result, "application/json");
        }



        [HttpGet("TaskAssignmentList")]
        public async Task<IActionResult> TaskAssignmentList(int Id)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:TaskAssignmentList"];
            var result = await GetToExternalService(productServiceUrl + Id.ToString());
            return Content(result, "application/json");
        }

         
        [HttpGet("UpdTaskAssignment")]
        public async Task<IActionResult> UpdTaskAssignment(int Id, string whoAssign, string ToWhomassign)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:UpdTaskAssignment"];
            var formattedUrl = productServiceUrl.Replace("{TaskId}", Id.ToString()).Replace("{whoAssign}", whoAssign.ToString()).Replace("{ToWhomassign}", ToWhomassign.ToString()); 
            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");
        }
          
         
        [HttpGet("AddTaskComment")]
        public async Task<IActionResult> AddTaskComment(int Id, string who, string Comment)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:AddTaskComment"];
            var formattedUrl = productServiceUrl.Replace("{TaskId}", Id.ToString()).Replace("{who}", who.ToString()).Replace("{Comment}", Comment.ToString());
            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");
        }



        [HttpGet("TaskCommentList")]
        public async Task<IActionResult> TaskCommentList(int Id)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:TaskCommentList"];
            var result = await GetToExternalService(productServiceUrl + Id.ToString());
            return Content(result, "application/json");
        }





        [HttpGet("DeleteTask")]
        public async Task<IActionResult> DeleteTask(int Id)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                         .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DeleteTask"];
            var formattedUrl = productServiceUrl.Replace("{TaskId}", Id.ToString()).Replace("{userId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl); 
            return Content(result, "application/json");
        }



        [HttpGet("TaskDocumentList")]
        public async Task<IActionResult> TaskDocumentList(int Id)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:TaskDocumentList"];
            var result = await GetToExternalService(productServiceUrl + Id.ToString());
            return Content(result, "application/json");
        }


        [HttpGet("GetCustomer")]
        public async Task<IActionResult> GetCustomer(string Type)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetCustomer"];
            var result = await GetToExternalService(productServiceUrl + Type.ToString()); 
            return Content(result, "application/json");
        }

        [HttpGet("DashboardPart1")]
        public async Task<IActionResult> DashboardPart1()
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;

            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DashboardPart1"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl);

            //var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");
        }

        [HttpGet("DashboardPart2")]
        public async Task<IActionResult> DashboardPart2()
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;

            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DashboardPart2"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl);

            // var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");
        }
        [HttpGet("DashboardPart3")]
        public async Task<IActionResult> DashboardPart3()
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;

            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DashboardPart3"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl);

            //var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");
        }
        [HttpGet("DashboardPart4")]
        public async Task<IActionResult> DashboardPart4()
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)

                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DashboardPart4"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl);

            //var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");
        }
        [HttpGet("DashboardPart5")]
        public async Task<IActionResult> DashboardPart5()
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;

            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DashboardPart5"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl);

            //var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");
        }
        [HttpGet("DashboardPart6")]
        public async Task<IActionResult> DashboardPart6()
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;

            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DashboardPart6"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl);

            // var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");
        }
        [HttpGet("DashboardPart7")]
        public async Task<IActionResult> DashboardPart7()
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;

            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DashboardPart7"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl);

            // var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");
        }

        [HttpGet("Downloadtemplate")]
        public async Task<IActionResult> Downloadtemplate(string filename)
        {
       
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:Downloadtemplate"];
            var result = await GetToExternalServiceforDownload(productServiceUrl, filename.ToString());
            return File(result, "application/octet-stream", filename);
        }

        #endregion
        #region This section is used for Task Comment

        [HttpGet("GetAllComment")]
        public async Task<IActionResult> GetAllComment(int TaskId)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetAllComment"];
            var result = await GetToExternalService(productServiceUrl + TaskId.ToString());
            return Content(result, "application/json");

        }

        [HttpGet("GetComment")]
        public async Task<IActionResult> GetComment(int TaskId, int CommentId)
        {
            var  productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetComment"]; 
            // Replace placeholders with actual values
            var formattedUrl = productServiceUrl.Replace("{TaskId}", TaskId.ToString())
                               .Replace("{CommentId}",CommentId.ToString());  

            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");

        }
        [HttpPost("NewComment")]
        public async Task<IActionResult> NewComment([FromBody] Comment Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:NewComment"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PostToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }

        [HttpPatch("UpdComment")]
        public async Task<IActionResult> UpdComment([FromBody] Comment tsk)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:UpdComment"];
            string jsonData = JsonConvert.SerializeObject(tsk);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
        [HttpPatch("DelComments")]
        public async Task<IActionResult> DelComments([FromBody] Comment tsk)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DelComment"];
            string jsonData = JsonConvert.SerializeObject(tsk);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
        #endregion
        #region This section is used for Project Document

        [HttpGet("GetAllDocument")]
        public async Task<IActionResult> GetAllDocument(int TaskId)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetAllDocument"];
            var result = await GetToExternalService(productServiceUrl+TaskId.ToString());
            return Content(result, "application/json");

        }

        [HttpGet("GetDocument")]
        public async Task<IActionResult> GetDocument(int TaskId, int DocumentId)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetDocument"]; 
            // Replace placeholders with actual values
            var formattedUrl = productServiceUrl.Replace("{TaskId}", TaskId.ToString())
                               .Replace("{DocumentId}", DocumentId.ToString());
            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");

        }
       
        [HttpPost("NewDocument")]
        public async Task<IActionResult> NewDocument([FromBody] Document Proj)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                         .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            Proj.CREATEDBY = userId;
            Proj.UPDATEDBY = userId;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:NewDocument"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PostToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }

        [HttpPatch("UpdDocument")]
        public async Task<IActionResult> UpdDocument([FromBody] Document Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:UpdDocument"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
        [HttpPatch("DelDocument")]
        public async Task<IActionResult> DelDocument([FromBody] Document Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DelDocument"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
         
        [HttpPost("uploadDoc")]
        public async Task<IActionResult> UploadDoc(IFormFile file, int taskId)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file uploaded." });

            if (taskId == 0)
                return BadRequest(new { message = "Task Not Found." });

            // 1. Get authenticated user name
            var username = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized(new { message = "User not authenticated." });

            // 2. Get user ID from DB
            var userId = _context.Set<UserIdResultDTO>()
                .FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                .AsEnumerable()
                .FirstOrDefault()?.UserId ?? 0;

            if (userId == 0)
                return NotFound(new { message = "User ID not found." });

            // 3. Prepare request to external API
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:uploadDoc"];

            // Add query parameters to the base URL
            var fullUrl = $"{productServiceUrl}?userId={userId}&taskId={taskId}";

            using var client = new HttpClient();
            using var formData = new MultipartFormDataContent();
            using var fileContent = new StreamContent(file.OpenReadStream());

            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            formData.Add(fileContent, "file", file.FileName);

            // POST the file to the upload endpoint
            var response = await client.PostAsync(fullUrl, formData);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { message = "File upload failed.", error });
            }

            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }


        [HttpPost("uploadTaskDoc")]
        public async Task<IActionResult> uploadTaskDoc(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file uploaded." });

            // 1. Get authenticated user name
            var username = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized(new { message = "User not authenticated." });

            // 2. Get user ID from DB
            var userId = _context.Set<UserIdResultDTO>()
                .FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                .AsEnumerable()
                .FirstOrDefault()?.UserId ?? 0;

            if (userId == 0)
                return NotFound(new { message = "User ID not found." });

            // 3. Prepare request to external API
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:uploadTaskDoc"];

            // Add query parameters to the base URL
            var fullUrl = $"{productServiceUrl}?userId={userId}";

            using var client = new HttpClient();
            using var formData = new MultipartFormDataContent();
            using var fileContent = new StreamContent(file.OpenReadStream());

            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            formData.Add(fileContent, "file", file.FileName);

            // POST the file to the upload endpoint
            var response = await client.PostAsync(fullUrl, formData);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { message = "File upload failed.", error });
            }

            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }
        #endregion
        #region This section is used for Task Status Statement  

        [HttpGet("GetAllTaskStatus")]
        public async Task<IActionResult> GetAllTaskStatus()
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetAllTaskStatus"];
            var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");

        }

        [HttpGet("GetTaskStatus")]
        public async Task<IActionResult> GetTaskStatus(int Id)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetTaskStatus"];
            var result = await GetToExternalService(productServiceUrl + Id.ToString());
            return Content(result, "application/json");

        }
        [HttpPost("NewTaskStatus")]
        public async Task<IActionResult> NewTaskStatus([FromBody] TasskStatus tsk)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:NewTaskStatus"];
            string jsonData = JsonConvert.SerializeObject(tsk);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PostToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }

        [HttpPatch("UpdTaskStatus")]
        public async Task<IActionResult> UpdTaskStatus([FromBody] TasskStatus tsk)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:UpdTaskStatus"];
            string jsonData = JsonConvert.SerializeObject(tsk);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
        [HttpPatch("DelTaskStatus")]
        public async Task<IActionResult> DelTaskStatus([FromBody] TasskStatus tsk)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DelTaskStatus"];
            string jsonData = JsonConvert.SerializeObject(tsk);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
        #endregion
        #region This section is used for General Status    

        [HttpGet("GetAllStatus")]
        public async Task<IActionResult> GetAllStatus()
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetAllStatus"];
            var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");

        }

        [HttpGet("GetStatus")]
        public async Task<IActionResult> GetStatus(int Id)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetStatus"];
            var result = await GetToExternalService(productServiceUrl + Id.ToString());
            return Content(result, "application/json");

        }
       
        [HttpPost("Newstatus")]
        public async Task<IActionResult> Newstatus([FromBody] STATUS Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:Newstatus"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PostToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }

        [HttpPatch("Updstatus")]
        public async Task<IActionResult> Updstatus([FromBody] STATUS Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:Updstatus"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
        
        [HttpPatch("Delstatus")]
        public async Task<IActionResult> Delstatus([FromBody] STATUS Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:Delstatus"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
        #endregion
        #region This section is used for Task Category Statement  

        [HttpGet("GetAllTASKCATEGORY")]
        public async Task<IActionResult> GetAllTASKCATEGORY()
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetAllTASKCATEGORY"];
            var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");

        }

        [HttpGet("GetTASKCATEGORY")]
        public async Task<IActionResult> GetTASKCATEGORY(int Id)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:GetTASKCATEGORY"];
            var result = await GetToExternalService(productServiceUrl + Id.ToString());
            return Content(result, "application/json");

        }
       
        [HttpPost("NewTASKCATEGORY")]
        public async Task<IActionResult> NewTASKCATEGORY([FromBody] TASKCATEGORY Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:NewTASKCATEGORY"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PostToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }

        [HttpPatch("UpdTASKCATEGORY")]
        public async Task<IActionResult> UpdTASKCATEGORY([FromBody] TASKCATEGORY Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:UpdTASKCATEGORY"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
        
        [HttpPatch("DelTASKCATEGORY")]
        public async Task<IActionResult> DelTASKCATEGORY([FromBody] TASKCATEGORY Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DelTASKCATEGORY"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
        #endregion
        #region This section is used for Task Status Statement  

        [HttpGet("AllTASKSPRIORITY")]
        public async Task<IActionResult> AllTASKSPRIORITY()
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:AllTASKSPRIORITY"];
            var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");

        }

        [HttpGet("TASKSPRIORITY")]
        public async Task<IActionResult> TASKSPRIORITY(int Id)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:TASKSPRIORITY"];
            var result = await GetToExternalService(productServiceUrl + Id.ToString());
            return Content(result, "application/json");

        }
      
        [HttpPost("NewTASKSPRIORITY")]
        public async Task<IActionResult> NewTASKSPRIORITY([FromBody] TASKSPRIORITY Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:NewTASKSPRIORITY"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PostToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }

        [HttpPatch("UpdTASKSPRIORITY")]
        public async Task<IActionResult> UpdTASKSPRIORITY([FromBody] TASKSPRIORITY Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:UpdTASKSPRIORITY"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
      
        [HttpPatch("DelTASKSPRIORITY")]
        public async Task<IActionResult> DelTASKSPRIORITY([FromBody] TASKSPRIORITY Proj)
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:DelTASKSPRIORITY"];
            string jsonData = JsonConvert.SerializeObject(Proj);
            // Create HttpContent
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var result = await PATCHToExternalService(productServiceUrl, content);
            return Content(result, "application/json");
        }
        #endregion
        #region Analytics Part

        [HttpGet("AnalyticsPart1")]
        public async Task<IActionResult> AnalyticsPart1(string val ,string Type )
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:AnalyticsPart1"];
            var formattedUrl = productServiceUrl.Replace("{Type}", Type.ToString()).Replace("{val}", val.ToString()).Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl); 
            return Content(result, "application/json");
        }
        [HttpGet("AnalyticsPart2")]
        public async Task<IActionResult> AnalyticsPart2(string val, string Type)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:AnalyticsPart2"];
            var formattedUrl = productServiceUrl.Replace("{Type}", Type.ToString()).Replace("{val}", val.ToString()).Replace("{UserId}", userId.ToString()); ;
            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");
        }
        [HttpGet("AnalyticsPart3")]
        public async Task<IActionResult> AnalyticsPart3(string val, string Type)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:AnalyticsPart3"];
            var formattedUrl = productServiceUrl.Replace("{Type}", Type.ToString()).Replace("{val}", val.ToString()).Replace("{UserId}", userId.ToString()); ;
            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");
        }
        [HttpGet("AnalyticsPart4")]
        public async Task<IActionResult> AnalyticsPart4(string val, string Type)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:AnalyticsPart4"];
            var formattedUrl = productServiceUrl.Replace("{Type}", Type.ToString()).Replace("{val}", val.ToString()).Replace("{UserId}", userId.ToString()); ;
            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");
        }
        [HttpGet("AnalyticsPart5")]
        public async Task<IActionResult> AnalyticsPart5(string val, string Type)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:AnalyticsPart5"];
            var formattedUrl = productServiceUrl.Replace("{Type}", Type.ToString()).Replace("{val}", val.ToString()).Replace("{UserId}", userId.ToString()); ;
            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");
        }
        [HttpGet("AnalyticsPart6")]
        public async Task<IActionResult> AnalyticsPart6(string val, string Type)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:AnalyticsPart6"];
            var formattedUrl = productServiceUrl.Replace("{Type}", Type.ToString()).Replace("{val}", val.ToString()).Replace("{UserId}", userId.ToString()); ;
            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");
        }
        [HttpGet("AnalyticsPart7")]
        public async Task<IActionResult> AnalyticsPart7(string val, string Type)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:AnalyticsPart7"];
            var formattedUrl = productServiceUrl.Replace("{Type}", Type.ToString()).Replace("{val}", val.ToString()).Replace("{UserId}", userId.ToString()); ;
            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");
        }
        [HttpGet("AnalyticsPart8")]
        public async Task<IActionResult> AnalyticsPart8(string val, string Type)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:AnalyticsPart8"];
            var formattedUrl = productServiceUrl.Replace("{Type}", Type.ToString()).Replace("{val}", val.ToString()).Replace("{UserId}", userId.ToString()); ;
            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");
        }
        [HttpGet("AnalyticsPart9")]
        public async Task<IActionResult> AnalyticsPart9(string val, string Type)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:AnalyticsPart9"];
            var formattedUrl = productServiceUrl.Replace("{Type}", Type.ToString()).Replace("{val}", val.ToString()).Replace("{UserId}", userId.ToString()); ;
            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");
        }
        [HttpGet("AnalyticsPart10")]
        public async Task<IActionResult> AnalyticsPart10(string val, string Type)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:AnalyticsPart10"];
            var formattedUrl = productServiceUrl.Replace("{Type}", Type.ToString()).Replace("{val}", val.ToString()).Replace("{UserId}", userId.ToString()); ;
            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");
        }
        #endregion
        #region HQ Part & TeamFeed & Agentqueue & Teamqueue
        [HttpGet("HQ")]
        public async Task<IActionResult> HQ()
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;

            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:HQ"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");



            //var result = await GetToExternalService(productServiceUrl);
            //return Content(result, "application/json");
        }
        [HttpGet("TeamFeed")]
        public async Task<IActionResult> TeamFeed()
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;

            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:TeamFeed"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl);
            //var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");
        }
        
        [HttpGet("Agentqueue")]
        public async Task<IActionResult> Agentqueue()
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;

            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:Agentqueue"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl);
            // var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");
        }

        [HttpGet("Teamqueue")]
        public async Task<IActionResult> Teamqueue()
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;

            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:Teamqueue"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString());
            var result = await GetToExternalService(formattedUrl);
            //var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");
        }



        [HttpGet("ProjectAssign")]
        public async Task<IActionResult> ProjectAssign()
        {
            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:ProjectAssign"];
            var result = await GetToExternalService(productServiceUrl);
            return Content(result, "application/json");
        }

        [HttpGet("ProjectAssignUpdate")]
        public async Task<IActionResult> ProjectAssignUpdate(string Developer,string ProjDetail)
        {
            var username = User.Identity?.Name;
            var userId = _context.Set<UserIdResultDTO>().FromSqlRaw("EXEC [TMS].[USP_GetUserId] @UserName = {0}", username)
                                .AsEnumerable().FirstOrDefault()?.UserId ?? 0;


            var productServiceUrl = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:ProjectAssignUpdate"];
            var formattedUrl = productServiceUrl.Replace("{UserId}", userId.ToString()).Replace("{Developer}", Developer.ToString()).Replace("{ProjDetail}", ProjDetail.ToString());
             var result = await GetToExternalService(formattedUrl);
            return Content(result, "application/json");
        }
        #endregion
        #region This section is common for User APi

        private async Task<string> PostToExternalService(string url ,dynamic obj)
        { 
            var client = _httpClient;
            var response = await client.PostAsync(url, obj);
            response.EnsureSuccessStatusCode(); // Throw on error code. 
            return await response.Content.ReadAsStringAsync();
        } 
        private async Task<string> GetToExternalService(string url)
        {
            var client = _httpClient;
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode(); // Throw on error code. 
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<byte[]> GetToExternalServiceforDownload(string url ,string filename)
        {
            var client = _httpClient;
            var response = await client.GetAsync(url+ filename);
            response.EnsureSuccessStatusCode(); // Throw on error code. 

            var contentDisposition = response.Content.Headers.ContentDisposition?.FileName?.Trim('"');
            var downloadFileName = !string.IsNullOrEmpty(contentDisposition) ? contentDisposition : filename;


           // var contentDisposition = response.Content.Headers.ContentDisposition?.FileName ?? "downloaded-file";
            return await response.Content.ReadAsByteArrayAsync();
          
        }
        private async Task<string> PUTToExternalService(string url, dynamic obj)
        {
            var client = _httpClient;
            var response = await client.PutAsync(url, obj);
            response.EnsureSuccessStatusCode(); // Throw on error code. 
            return await response.Content.ReadAsStringAsync();
        }
        private async Task<string> PATCHToExternalService(string url, dynamic obj)
        {
            var client = _httpClient;
            var response = await client.PatchAsync(url, obj);
            response.EnsureSuccessStatusCode(); // Throw on error code. 
            return await response.Content.ReadAsStringAsync();
        }
        #endregion
    }
}

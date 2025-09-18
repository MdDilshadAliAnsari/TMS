using Microsoft.AspNetCore.Mvc;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMS.DATA.Model;
using TMS.Domain;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using EAGetMail;
using Microsoft.Extensions.Hosting.Internal;

namespace TMS.DATA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailwithAttachmentController : ControllerBase
    {
        #region  Variable Constructor and Dependency injection in Constructor level
        private readonly TMSDbContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;

        public MailwithAttachmentController(TMSDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }


        #endregion
        [HttpGet("ImportMails")]
        public async Task<IActionResult> ImportMailwithAttach()
        {
            string[] scopes = new[] { "Mail.Read" };
            // Use PublicClientApplicationBuilder for interactive authentication (no client secret)
            var app = PublicClientApplicationBuilder.Create(Tsecret.ClientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, Tsecret.TenantId)
                 .WithDefaultRedirectUri()  // Use default redirect URI (http://localhost)
                                            //.WithRedirectUri("http://49.50.68.178:8084/swagger/index.html")
                                            //.WithDefaultRedirectUri()
                .Build();

            try
            {
                // Trigger interactive authentication
                var result = await app.AcquireTokenInteractive(scopes)
                                      .ExecuteAsync();

                // Access token received after successful login
                string accessToken = result.AccessToken;
                // Now you can use the token to call APIs (e.g., Microsoft Graph)
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await httpClient.GetAsync(Tsecret.Url);


                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error retrieving mails: {response.StatusCode} - {error}");
                }

                var json = await response.Content.ReadAsStringAsync();
                RootMsg emailData = JsonConvert.DeserializeObject<RootMsg>(json);


                var formatter = new Formatter();
                if (emailData?.value != null)
                {
                    foreach (var msg in emailData.value)
                    {

                        var mail = new MailData
                        {
                            MsgId           = msg.id,
                            FromemailAddress = msg.from.emailAddress.address.ToString(),
                            SenderemailAddress = msg.sender.emailAddress.address.ToString(),
                            ToRecipientemailAddress = formatter.GetFormatstr(msg.toRecipients.ToList()),
                            CcRecipientemailAddress = formatter.GetFormatstr(msg.ccRecipients.ToList()),
                            Subject = msg.subject,
                            BodycontentType = msg.body.contentType.ToString(),
                            Bodycontent = msg.body.content.ToString(),
                            ISMigrate = 1,
                            CREATEDBY = 1111,
                            CREATEDON = msg.receivedDateTime,
                            UPDATEDBY = 1111,
                            UPDATEDON = msg.receivedDateTime
                        };
                        _context.tblEmails.Add(mail);
                        _context.SaveChanges();


                        // 🌐 Call Microsoft Graph to get attachments for this message
                        // var attachmentResponse = await httpClient.GetAsync($"https://graph.microsoft.com/v1.0/me/messages/{msg.id}/attachments");

                        string messageId = msg.id;
                        string attachmentUrl = $"https://graph.microsoft.com/v1.0/me/messages/{messageId}/attachments";

                        var attachmentResponse = await httpClient.GetAsync(attachmentUrl);

                        if (attachmentResponse.IsSuccessStatusCode)
                        {
                            var listJson = await attachmentResponse.Content.ReadAsStringAsync();
                            var attachmentList = JsonConvert.DeserializeObject<AttachmentRoot>(listJson);
                            foreach (var attachmentInfo in attachmentList.value)
                            {
                                string attachmentId = attachmentInfo.id;
                                string detailUrl = $"https://graph.microsoft.com/v1.0/me/messages/{messageId}/attachments/{attachmentId}";

                                var detailResponse = await httpClient.GetAsync(detailUrl);

                                if (detailResponse.IsSuccessStatusCode)
                                {
                                    var detailJson = await detailResponse.Content.ReadAsStringAsync();
                                    var fileAttachment = JsonConvert.DeserializeObject<FileAttachment>(detailJson);

                                    if (!string.IsNullOrEmpty(fileAttachment.contentBytes))
                                    {
                                        string projectRootPath = _hostingEnvironment.ContentRootPath; 
                                        byte[] fileBytes = Convert.FromBase64String(fileAttachment.contentBytes);
                                       // string filePath = Path.Combine("D:\\Published\\TMS.Services\\Document\\", fileAttachment.name);
                                        string filePath = Path.Combine(projectRootPath, "MailAttachment", fileAttachment.name);
                                        System.IO.File.WriteAllBytes(filePath, fileBytes);

                                        var mailAttached = new MailAttachment
                                        {
                                            MsgId = msg.id,
                                            URL = filePath,
                                            CREATEDBY = 1111,
                                            CREATEDON = msg.receivedDateTime,
                                            UPDATEDBY = 1111,
                                            UPDATEDON = msg.receivedDateTime
                                        };
                                        _context.tblMailAttachment.Add(mailAttached);
                                        _context.SaveChanges();

                                        // string filePath = Path.Combine("D:\\Published\\TMS.Services\\Document\\", fileAttachment.name); 
                                        // System.IO.File.WriteAllBytes(filePath, fileBytes);
                                    }
                                }
                            }




                            ////////var attachmentJson = await attachmentResponse.Content.ReadAsStringAsync();
                            ////////var attachments = JsonConvert.DeserializeObject<AttachmentRoot>(attachmentJson);

                            ////////foreach (var attachment in attachments.value)
                            ////////{

                            ////////    if (attachment is FileAttachment fileAttachment && fileAttachment.contentBytes != null)
                            ////////    {
                            ////////        // Decode Base64 file content
                            ////////        byte[] fileBytes = Convert.FromBase64String(fileAttachment.contentBytes);

                            ////////        // Save file to disk or database as needed
                            ////////        string filePath = Path.Combine("D:\\Published\\TMS.Services\\Document\\", fileAttachment.name);
                            ////////        //await File.WriteAllBytesAsync(filePath, fileBytes); 
                            ////////        //File.WriteAllBytesAsync(filePath, fileBytes);

                            ////////    }
                            ////////}
                            //  var attachmentJson = await attachmentResponse.Content.ReadAsStringAsync();
                            //var attachmentRoot = JsonConvert.DeserializeObject<AttachmentRoot>(attachmentJson);

                            //if (attachmentRoot?.value != null)
                            // {
                            ////    foreach (var att in attachmentRoot.value)
                            ////    {
                            ////        // Example: Store base64 content in DB or file system
                            ////        var attachment = new MailAttachment
                            ////        {
                            ////            EmailId = mail.MailDataId, // foreign key to MailData
                            ////            Name = att.name,
                            ////            ContentBytes = att.contentBytes,
                            ////            ContentType = att.contentType,
                            ////            Size = att.size,
                            ////            CreatedOn = DateTime.UtcNow
                            ////        };

                            ////        _context.tblEmailAttachments.Add(attachment);
                            ////        await _context.SaveChangesAsync();
                            ////    }
                            // }
                        }
                            }
                }
                return Ok(new { message = json });

            }
            catch (MsalUiRequiredException msalEx)
            {
                // This exception is thrown when the user cancels the authentication or closes the browser
                Console.WriteLine($"Authentication UI required but was cancelled: {msalEx.Message}");
                return BadRequest(new { Message = "Authentication was cancelled or the user closed the browser." });
            }
            catch (MsalServiceException msalServiceEx)
            {
                // This exception is thrown if there's an issue with the MSAL service
                Console.WriteLine($"Authentication failed due to service error: {msalServiceEx.Message}");
                return BadRequest(new { Message = "Authentication failed due to service error." });
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine($"General error: {ex.Message}");
                return BadRequest(new { Message = $"Authentication failed: {ex.Message}" });
            }
        }

    }


    //public class AttachmentRoot
    //{
    //    public List<MailAttachment> value { get; set; }
    //}

    //public class Attachment
    //{
    //    public string id { get; set; }
    //    public string name { get; set; }
    //    public string contentType { get; set; }
    //    public int size { get; set; }
    //    public string contentBytes { get; set; }  // Base64 encoded
    //}
    public class AttachmentRoot
    {
        public List<AttachmentBase> value { get; set; }
    }

    public class AttachmentBase
    {
        public string id { get; set; }
        public string name { get; set; }
        public string contentType { get; set; }
        public int size { get; set; }
       
        public string contentBytes { get; set; }
    }

    public class FileAttachment : AttachmentBase
    {
        public string contentBytes { get; set; }
    }
}

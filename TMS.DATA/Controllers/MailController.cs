using  Microsoft.AspNetCore.Mvc;
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

namespace TMS.DATA.Controllers
{

    
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {


        #region  Variable Constructor and Dependency injection in Constructor level
        private readonly TMSDbContext _context; 
        public MailController(TMSDbContext context)
        {
            _context = context; 
        }
        #endregion

        #region DATA EXTRACTION from Mail
        [HttpGet("ImportMails")] 
        public async Task<IActionResult> ImportMails()
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
                            FromemailAddress            = msg.from.emailAddress.address.ToString(),
                            SenderemailAddress          = msg.sender.emailAddress.address.ToString(),
                            ToRecipientemailAddress     = formatter.GetFormatstr(msg.toRecipients.ToList()),
                            CcRecipientemailAddress     = formatter.GetFormatstr( msg.ccRecipients.ToList()),
                            Subject                     = msg.subject,
                            BodycontentType             = msg.body.contentType.ToString(),
                            Bodycontent                 = msg.body.content.ToString(),
                            ISMigrate                   = 1,
                            CREATEDBY                   = 1111,
                            CREATEDON                   = msg.receivedDateTime,
                            UPDATEDBY                   = 1111,
                            UPDATEDON                   = msg.receivedDateTime
                        };
                        _context.tblEmails.Add(mail);
                        _context.SaveChanges(); 
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

        #endregion
    }

        #region Supported Claas for Extracting  Data
    public class Formatter
{
        public string GetFormatstr<T>(List<T> items)
        {
            if (items == null || !items.Any())
                return "No data available.";

            var builder = new StringBuilder();

            foreach (var item in items)
            {
                builder.AppendLine(FormatProperties(item));
                builder.AppendLine(new string('-', 40));
            }

            return builder.ToString();
        }

        private string FormatProperties(object obj, int indent = 0)
        {
            if (obj == null) return string.Empty;

            var builder = new StringBuilder();
            var type = obj.GetType();

            foreach (var prop in type.GetProperties())
            {
                var value = prop.GetValue(obj);
                string indentStr = new string(' ', indent * 2);

                if (value != null && !prop.PropertyType.Namespace.StartsWith("System"))
                {
                    // It's a nested custom class
                    builder.AppendLine($"{indentStr}{prop.Name}:");
                    builder.Append(FormatProperties(value, indent + 1));
                }
                else
                {
                    builder.AppendLine($"{indentStr}{prop.Name}: {value}");
                }
            }

            return builder.ToString();
        }

    }
    #endregion
}





using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TMS.Authentication.Model;
using TMS.Authentication.Notification;

namespace TMS.Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly TMSDbContext _context;
        public NotificationController(EmailService emailService, TMSDbContext context)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _context = context;
        }

        [HttpPost("sendwelcomeMail")]
        public async Task<IActionResult> SendWelcomeEmail([FromQuery] string email, [FromQuery] string name)
        {
            
            try
            {
                _emailService.SendWelcomeEmail(email, name);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw;
            }
            return Ok(new { message = "Email sent successfully.!!!" });
     
        }

        [HttpPost("sendPwdMail")]
        public async Task<IActionResult> SendpwdEmail([FromQuery] string email, [FromQuery] string name, [FromQuery] string pwd)
        {

            try
            {
                _emailService.SendPwdEmail(email, name,pwd);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw;
            }

            return Ok(new { message = "Email sent successfully.!!!" });
        }

        [HttpPost("sendnewPwdMail")]
        public async Task<IActionResult> ChangePwdEmail([FromQuery] string email, [FromQuery] string name, [FromQuery] string pwd)
        {

            try
            {
                _emailService.ChangePwdEmail(email, name, pwd);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw;
            }

            return Ok(new { message = "Email sent successfully.!!!" });
        }
         
        [HttpPost("SendNewTaskAssignMail")]
        public IActionResult SendNewTaskAssignMail(int taskid)
        {
            try
            {
                var objmail = _context.Set<ASSIGNEDEMAILDTO>()
                                      .FromSqlRaw("EXEC [TMS].[USP_DetailsAssignMail] @TaskId = {0}", taskid)
                                      .AsEnumerable()
                                      .FirstOrDefault();

                if (objmail == null)
                {
                    return NotFound(new { message = "No task details found for the given Task ID." });
                } 
                _emailService.SendNewTaskAssignMail(objmail);

                return Ok(new { message = "Email sent successfully!" });
            }
            catch (SmtpException ex)
            {
                // Log the SMTP error
                Console.WriteLine($"SMTP Error: {ex.Message}");
                return StatusCode(500, new { message = "Failed to send email due to SMTP error." });
            }
            catch (Exception ex)
            {
                // Log general error
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while sending the email." });
            }
        }

    }

}

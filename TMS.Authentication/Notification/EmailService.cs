 
 
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using TMS.Authentication.Model;

namespace TMS.Authentication.Notification
{
    public class EmailService
    {
        private readonly SmtpSettings _smtpSettings;
         
        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings?.Value ?? throw new ArgumentNullException(nameof(smtpSettings));
       
        }
        public async Task SendWelcomeEmail(string toEmail, string userName)
        {
            using (var client = new SmtpClient(_smtpSettings.SmtpClient, _smtpSettings.SMTPPort))
            {
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "welcome.html");
                string emailBody = await File.ReadAllTextAsync(templatePath);
                emailBody = emailBody.Replace("{{UserName}}", userName);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpSettings.UserID, _smtpSettings.Password);

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(_smtpSettings.FromAddress);
                    message.To.Add(toEmail);
                    message.Subject = "Welcome Ticket Management System[TMS]";
                    message.Body = emailBody;
                    message.IsBodyHtml = true;

                    client.Send(message);
                    Console.WriteLine("Email sent successfully!");
                }
            }

        }
        public async Task SendPwdEmail(string toEmail, string userName, string Pwd)
        {
            using (var client = new SmtpClient(_smtpSettings.SmtpClient, _smtpSettings.SMTPPort))
            {
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "forgetpwd.html");
                string emailBody = await File.ReadAllTextAsync(templatePath);
                emailBody = emailBody.Replace("{{UserName}}", userName);
                emailBody = emailBody.Replace("{{pwd}}", Pwd);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpSettings.UserID, _smtpSettings.Password);

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(_smtpSettings.FromAddress);
                    message.To.Add(toEmail);
                    message.Subject = "Ticket Management System[TMS] : Password Recover";
                    message.Body = emailBody;
                    message.IsBodyHtml = true;

                    client.Send(message);
                    Console.WriteLine("Email sent successfully!");
                }
            }
        }

        public async Task ChangePwdEmail(string toEmail, string userName, string Pwd)
        {
            using (var client = new SmtpClient(_smtpSettings.SmtpClient, _smtpSettings.SMTPPort))
            {
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "changepwd.html");
                string emailBody = await File.ReadAllTextAsync(templatePath);
                emailBody = emailBody.Replace("{{UserName}}", userName);
                emailBody = emailBody.Replace("{{pwd}}", Pwd);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpSettings.UserID, _smtpSettings.Password);

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(_smtpSettings.FromAddress);
                    message.To.Add(toEmail);
                    message.Subject = "Ticket Management System[TMS] : Your Password Was Recently Updated";
                    message.Body = emailBody;
                    message.IsBodyHtml = true;

                    client.Send(message);
                    Console.WriteLine("Email sent successfully!");
                }
            }
        }
         
        public async Task SendNewTaskAssignMail(ASSIGNEDEMAILDTO objmaill)
        {
            using (var client = new SmtpClient(_smtpSettings.SmtpClient, _smtpSettings.SMTPPort))
            {
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "AssignTask.html");
                string emailBody = await File.ReadAllTextAsync(templatePath);
                emailBody = emailBody.Replace("{{UserName}}",   objmaill.WHICHDEVELOPERASSIGNEDUSERNAME);
                emailBody = emailBody.Replace("{{TaskSNo}}",    objmaill.TASKSERIALNO);
                emailBody = emailBody.Replace("{{TaskTitle}}",  objmaill.TASKTITLE);
                emailBody = emailBody.Replace("{{TaskDesc}}",   objmaill.TASKDESC);
                emailBody = emailBody.Replace("{{ManagerName}}",objmaill.WHOASSIGNEDUSERNAME);
                emailBody = emailBody.Replace("{{DueDate}}",    objmaill.DUEDATE);
                emailBody = emailBody.Replace("{{Priority}}",   objmaill.PRIORITY);

                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpSettings.UserID, _smtpSettings.Password);

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(_smtpSettings.FromAddress);
                    message.To.Add(objmaill.WHICHDEVELOPERASSIGNEDEMAILID);
                    message.Subject = "Ticket Management System[TMS] : New Task Assignment: [ " + objmaill.TASKTITLE +" ]";
                    message.Body = emailBody;
                    message.IsBodyHtml = true;

                    client.Send(message);
                    Console.WriteLine("Email sent successfully!");
                }
            }
        }
    } 
    public class SmtpSettings
    {
        public string FromAddress { get; set; }
        public string SmtpClient { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public int SMTPPort { get; set; }
        public string  CC  { get; set; }
        public string EnableSSL { get; set; }
    }

}


 
 
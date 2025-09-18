using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TMS.Authentication.Authenticate;
using TMS.Authentication.Model;
using TMS.Domain;

namespace TMS.Authentication.Middleware
{
    public class RequestLoggingMiddleware
    {
        #region  Variable Constructor and Dependency injection in Constructor level
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _logger = logger;
            _httpClientFactory = httpClientFactory;

        }
        public async Task InvokeAsync(HttpContext context)
        {
            string userName = "Guest";
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var username = jwtToken.Claims
                    .FirstOrDefault(c => c.Type == "userName" || c.Type == "unique_name" || c.Type == ClaimTypes.Name)
                    ?.Value ?? "Guest";
                userName = username;
            }

            // 🟦 Log the incoming request method and URL
            var method = context.Request.Method;
            var url = context.Request.Path;

            var stopwatch = Stopwatch.StartNew();

            await _next(context);  // Pass to next middleware 
            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;
            if (method == "GET")
            {
                var log = new ExceptionLogger
                {

                    LoggerId = 0,
                    UserName = "Event Loggers" + " : " + userName,
                    ExceptionType = "LOGGERS",
                    ExceptionMessage = "REQ",
                    ControllerName = url,
                    ActionName = method,
                    IP = context.Connection.RemoteIpAddress.ToString(),
                    ExceptionStackTrace = "Incoming Request: Method: " + method + " & Url : " + url,
                    LogTime = DateTime.Now,

                };
                var client = _httpClientFactory.CreateClient(TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:Loggers"].ToString());
                var content = new StringContent(JsonSerializer.Serialize(log), Encoding.UTF8, "application/json");
                // Make async POST request to log server
                var response = await client.PostAsync(TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:Loggers"].ToString().ToString(), content);
                //-- Temporary comment---// _logger.LogInformation("Incoming Request: {Method} {Url}", method, url);
            }
            else
            {
                var log1 = new ExceptionLogger
                {

                    LoggerId = 0,
                    UserName = "Event Loggers"+ " : " + userName,
                    ExceptionType = "LOGGERS",
                    ExceptionMessage = "RESP",
                    ControllerName = url,
                    ActionName = method,
                    IP = context.Connection.RemoteIpAddress.ToString(),
                    ExceptionStackTrace = "✅ Finished handling request.Status code:" + context.Response.StatusCode + " , Time: { elapsed}ms :" + stopwatch.ElapsedMilliseconds,
                    LogTime = DateTime.Now,

                };
                var client1 = _httpClientFactory.CreateClient(TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:Loggers"].ToString());
                var content1 = new StringContent(JsonSerializer.Serialize(log1), Encoding.UTF8, "application/json");
                // Make async POST request to log server
                var response1 = await client1.PostAsync(TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["URL:Loggers"].ToString(), content1);
                //-- Temporary comment---//_logger.LogInformation("✅ Finished handling request. Status code: {statusCode}, Time: {elapsed} ms", context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
            }
        }

    }
}

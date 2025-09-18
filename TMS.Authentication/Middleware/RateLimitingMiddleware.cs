using System.Collections.Concurrent;
using System.Net;

namespace TMS.Authentication.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, RateLimitInfo> _clients = new();
        private readonly int _maxRequests;
        private readonly TimeSpan _timeWindow;

        public RateLimitingMiddleware(RequestDelegate next, int maxRequests, TimeSpan timeWindow)
        {
            _next = next;
            _maxRequests = maxRequests;
            _timeWindow = timeWindow;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var now = DateTime.UtcNow;

            var client = _clients.GetOrAdd(clientIp, new RateLimitInfo(now, 0));

            lock (client)
            {
                if ((now - client.StartTime) > _timeWindow)
                {
                    client.StartTime = now;
                    client.RequestCount = 0;
                }

                client.RequestCount++;

                if (client.RequestCount > _maxRequests)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.Response.Headers["Retry-After"] = _timeWindow.TotalSeconds.ToString();
                    return;
                }
            }

            await _next(context);
        }

        private class RateLimitInfo
        {
            public DateTime StartTime;
            public int RequestCount;

            public RateLimitInfo(DateTime startTime, int requestCount)
            {
                StartTime = startTime;
                RequestCount = requestCount;
            }
        }
    }
}

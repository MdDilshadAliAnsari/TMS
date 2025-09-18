using Microsoft.AspNetCore.Builder;
using System;

namespace TMS.Authentication.Middleware
{
    public static class RateLimitingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRateLimiting(
            this IApplicationBuilder builder,
            int maxRequests,
            TimeSpan timeWindow)
        {
            return builder.UseMiddleware<RateLimitingMiddleware>(maxRequests, timeWindow);
        }
    }
}

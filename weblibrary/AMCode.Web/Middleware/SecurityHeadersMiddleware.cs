using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace AMCode.Web.Middleware
{
    /// <summary>
    /// Middleware for adding security headers to HTTP responses
    /// </summary>
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the SecurityHeadersMiddleware class
        /// </summary>
        /// <param name="next">The next middleware in the pipeline</param>
        /// <param name="environment">Host environment to determine development vs production settings</param>
        public SecurityHeadersMiddleware(RequestDelegate next, IHostEnvironment environment)
        {
            _next = next;
            _environment = environment;
        }

        /// <summary>
        /// Invokes the middleware to add security headers to the response
        /// </summary>
        /// <param name="context">HTTP context</param>
        public async Task InvokeAsync(HttpContext context)
        {
            // Skip security headers for OPTIONS preflight requests (CORS handles these)
            // But add cache-control headers to prevent browser from caching failed CORS responses
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
                context.Response.Headers.Append("Pragma", "no-cache");
                context.Response.Headers.Append("Expires", "0");
                await _next(context);
                return;
            }

            // Add security headers
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "DENY");
            context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
            context.Response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=(), camera=()");

            // Content Security Policy (adjust based on your needs)
            // Note: In development, we allow HTTP connections for localhost
            var csp = _environment.IsDevelopment()
                ? "default-src 'self'; " +
                  "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
                  "style-src 'self' 'unsafe-inline'; " +
                  "img-src 'self' data: https: http:; " +
                  "font-src 'self' data:; " +
                  "connect-src 'self' https: http://localhost:* http://127.0.0.1:*;"
                : "default-src 'self'; " +
                  "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
                  "style-src 'self' 'unsafe-inline'; " +
                  "img-src 'self' data: https:; " +
                  "font-src 'self' data:; " +
                  "connect-src 'self' https:;";

            context.Response.Headers.Append("Content-Security-Policy", csp);

            await _next(context);
        }
    }
}

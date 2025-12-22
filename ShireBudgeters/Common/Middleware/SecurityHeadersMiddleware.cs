namespace ShireBudgeters.Common.Middleware;

/// <summary>
/// Middleware to add security headers to all HTTP responses.
/// These headers help protect against common web vulnerabilities.
/// </summary>
public class SecurityHeadersMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        // Add security headers
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
        context.Response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=(), camera=()");

        // Content Security Policy - adjust based on your application's needs
        // This is a restrictive CSP that may need adjustment for your specific use case
        var csp = "default-src 'self'; " +
                  "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdn.jsdelivr.net; " + // Bootstrap 5 JS and Blazor
                  "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " + // Bootstrap 5 CSS
                  "img-src 'self' data: https:; " +
                  "font-src 'self' data: https://cdn.jsdelivr.net; " + // Bootstrap 5 fonts if needed
                  "connect-src 'self' https:; " +
                  "frame-ancestors 'none';";
        
        context.Response.Headers.Append("Content-Security-Policy", csp);

        // Strict Transport Security (HSTS) - only add in production via UseHsts()
        // This is handled separately in Program.cs for production environments

        await _next(context);
    }
}


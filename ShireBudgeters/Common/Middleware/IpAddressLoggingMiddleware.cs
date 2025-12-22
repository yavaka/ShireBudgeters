using Serilog.Context;
using System.Net;

namespace ShireBudgeters.Common.Middleware;

/// <summary>
/// Middleware to capture and log the client IP address for every request.
/// </summary>
public class IpAddressLoggingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        // Get the client IP address
        var ipAddress = GetClientIpAddress(context);

        // Add IP address to the Serilog log context
        using (LogContext.PushProperty("IpAddress", ipAddress))
        {
            await _next(context);
        }
    }

    /// <summary>
    /// Extracts the client IP address from the HTTP context, considering proxy headers.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>The client IP address as a string.</returns>
    private static string GetClientIpAddress(HttpContext context)
    {
        // Check for IP address in X-Forwarded-For header (for reverse proxies/load balancers)
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            // X-Forwarded-For can contain multiple IPs, the first one is the original client
            var ips = forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (ips.Length > 0 && IPAddress.TryParse(ips[0], out _))
            {
                return ips[0];
            }
        }

        // Check for IP address in X-Real-IP header (alternative proxy header)
        var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp) && IPAddress.TryParse(realIp, out _))
        {
            return realIp;
        }

        // Check for IP address in CF-Connecting-IP header (Cloudflare)
        var cfConnectingIp = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(cfConnectingIp) && IPAddress.TryParse(cfConnectingIp, out _))
        {
            return cfConnectingIp;
        }

        // Fall back to the remote IP address from the connection
        var remoteIp = context.Connection.RemoteIpAddress;
        if (remoteIp != null)
        {
            // Handle IPv4-mapped IPv6 addresses
            if (remoteIp.IsIPv4MappedToIPv6)
            {
                return remoteIp.MapToIPv4().ToString();
            }
            return remoteIp.ToString();
        }

        return "Unknown";
    }
}


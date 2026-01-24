namespace ShireBudgeters.Common.Common.Constants;

/// <summary>
/// Constants for post-related validation, security, and configuration values.
/// </summary>
public static class Post
{
    /// <summary>
    /// Maximum allowed length for meta description in characters.
    /// </summary>
    public const int MetaDescriptionMaxLength = 300;

    /// <summary>
    /// Maximum number of recent posts that can be retrieved in a single request.
    /// </summary>
    public const int MaxRecentPostsCount = 100;

    /// <summary>
    /// Minimum number of recent posts that can be retrieved in a single request.
    /// </summary>
    public const int MinRecentPostsCount = 1;

    /// <summary>
    /// Regular expression pattern for validating URL-friendly slugs.
    /// Allows lowercase letters, numbers, and hyphens. Does not allow consecutive hyphens or leading/trailing hyphens.
    /// </summary>
    public const string SlugPattern = @"^[a-z0-9]+(?:-[a-z0-9]+)*$";

    /// <summary>
    /// Path traversal pattern used to detect directory traversal attacks.
    /// </summary>
    public const string PathTraversalPattern = "..";

    /// <summary>
    /// HTTP scheme identifier.
    /// </summary>
    public const string HttpScheme = "http";

    /// <summary>
    /// HTTPS scheme identifier.
    /// </summary>
    public const string HttpsScheme = "https";

    /// <summary>
    /// Allowed domains for featured images. These domains are trusted for external image URLs.
    /// </summary>
    public static readonly string[] AllowedImageDomains = 
    {
        "localhost",
        "127.0.0.1",
        // Add production domains here
    };

    /// <summary>
    /// Allowed relative paths for featured images. Images from these paths are considered safe.
    /// </summary>
    public static readonly string[] AllowedImagePaths = 
    {
        "/images/",
        "/img/",
        "/assets/",
        "/wwwroot/",
    };

    /// <summary>
    /// Dangerous URL patterns that could be used for URL injection attacks.
    /// These patterns are checked in slugs and URLs to prevent security vulnerabilities.
    /// </summary>
    public static readonly string[] DangerousUrlPatterns = 
    {
        @"\.\./",           // Path traversal
        @"\.\.\\",          // Path traversal (Windows)
        @"%2e%2e%2f",       // URL-encoded path traversal
        @"%2e%2e%5c",       // URL-encoded path traversal (Windows)
        @"javascript:",     // JavaScript protocol
        @"data:",           // Data URI
        @"vbscript:",      // VBScript protocol
        @"on\w+\s*=",       // Event handlers (onclick, onerror, etc.)
    };

    /// <summary>
    /// Dangerous XSS patterns that are removed from content body to prevent cross-site scripting attacks.
    /// </summary>
    public static readonly string[] DangerousXssPatterns = 
    {
        @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>",  // Script tags
        @"javascript:",                                         // JavaScript protocol
        @"on\w+\s*=",                                          // Event handlers
        @"<iframe\b",                                          // Iframe tags
        @"<object\b",                                           // Object tags
        @"<embed\b",                                           // Embed tags
    };
}

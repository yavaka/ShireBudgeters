namespace ShireBudgeters.Common.Common.Constants;

/// <summary>
/// Constants for lead magnet-related validation, security, and configuration values.
/// </summary>
public static class LeadMagnet
{
    /// <summary>
    /// Maximum allowed length for lead magnet title in characters.
    /// </summary>
    public const int TitleMaxLength = 255;

    /// <summary>
    /// Maximum allowed length for form action URL in characters.
    /// </summary>
    public const int FormActionUrlMaxLength = 500;

    /// <summary>
    /// Maximum allowed length for download file URL in characters.
    /// </summary>
    public const int DownloadFileUrlMaxLength = 500;

    /// <summary>
    /// Dangerous XSS patterns that are removed from lead magnet title to prevent cross-site scripting attacks.
    /// </summary>
    public static readonly string[] DangerousXssPatterns = 
    {
        @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>",  // Script tags
        @"javascript:",                                         // JavaScript protocol
        @"on\w+\s*=",                                          // Event handlers
        @"<iframe\b",                                          // Iframe tags
        @"<object\b",                                           // Object tags
        @"<embed\b",                                           // Embed tags
        @"<style\b[^<]*(?:(?!<\/style>)<[^<]*)*<\/style>",    // Style tags
    };
}


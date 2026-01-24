namespace ShireBudgeters.Common.Common.Constants;

/// <summary>
/// Constants for category-related validation, security, and configuration values.
/// </summary>
public static class Category
{
    /// <summary>
    /// Maximum allowed length for category name in characters.
    /// </summary>
    public const int NameMaxLength = 100;

    /// <summary>
    /// Maximum allowed length for category description in characters.
    /// </summary>
    public const int DescriptionMaxLength = 500;

    /// <summary>
    /// Maximum allowed length for category color in characters.
    /// </summary>
    public const int ColorMaxLength = 50;

    /// <summary>
    /// Maximum allowed length for user ID in characters.
    /// </summary>
    public const int UserIdMaxLength = 450;

    /// <summary>
    /// Maximum allowed length for audit fields (CreatedBy, ModifiedBy) in characters.
    /// </summary>
    public const int AuditFieldMaxLength = 100;

    /// <summary>
    /// Regular expression pattern for validating hex color codes (e.g., #FF0000 or #fff).
    /// </summary>
    public const string HexColorPattern = @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";

    /// <summary>
    /// Regular expression pattern for validating CSS color names (basic validation).
    /// This is a simplified pattern - full validation would require a comprehensive list of CSS color names.
    /// </summary>
    public const string CssColorNamePattern = @"^[a-zA-Z]+$";

    /// <summary>
    /// Dangerous XSS patterns that are removed from category name and description to prevent cross-site scripting attacks.
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


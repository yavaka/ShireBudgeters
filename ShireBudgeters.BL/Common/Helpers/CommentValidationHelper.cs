using Ganss.Xss;
using ShireBudgeters.Common.Common.Constants;
using System.Text.RegularExpressions;

namespace ShireBudgeters.BL.Common.Helpers;

/// <summary>
/// Helper for comment validation and sanitization using HtmlSanitizer (Ganss.Xss).
/// </summary>
public static class CommentValidationHelper
{
    /// <summary>
    /// Basic email format pattern for server-side validation.
    /// </summary>
    private static readonly Regex EmailPattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    /// <summary>
    /// Sanitizer for ContentBody with minimal whitelist: strong, em, a, br for limited formatting.
    /// </summary>
    private static readonly HtmlSanitizer ContentBodySanitizer = CreateContentBodySanitizer();

    /// <summary>
    /// Sanitizer for plain text fields (AuthorName, AuthorEmail) - strips all HTML.
    /// </summary>
    private static readonly HtmlSanitizer PlainTextSanitizer = CreatePlainTextSanitizer();

    private static HtmlSanitizer CreateContentBodySanitizer()
    {
        var sanitizer = new HtmlSanitizer();
        sanitizer.AllowedTags.Clear();
        sanitizer.AllowedTags.Add("strong");
        sanitizer.AllowedTags.Add("em");
        sanitizer.AllowedTags.Add("a");
        sanitizer.AllowedTags.Add("br");
        sanitizer.AllowedAttributes.Add("href"); // for <a href="...">
        sanitizer.AllowedSchemes.Add("http");
        sanitizer.AllowedSchemes.Add("https");
        return sanitizer;
    }

    private static HtmlSanitizer CreatePlainTextSanitizer()
    {
        var sanitizer = new HtmlSanitizer();
        sanitizer.AllowedTags.Clear();
        return sanitizer;
    }

    /// <summary>
    /// Validates author name and throws if invalid.
    /// </summary>
    public static void ValidateAuthorName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Author name is required.", nameof(name));
        }

        if (name.Length > Comment.AuthorNameMaxLength)
        {
            throw new ArgumentException($"Author name cannot exceed {Comment.AuthorNameMaxLength} characters.", nameof(name));
        }
    }

    /// <summary>
    /// Validates author email and throws if invalid.
    /// </summary>
    public static void ValidateAuthorEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Author email is required.", nameof(email));
        }

        if (email.Length > Comment.AuthorEmailMaxLength)
        {
            throw new ArgumentException($"Author email cannot exceed {Comment.AuthorEmailMaxLength} characters.", nameof(email));
        }

        if (!EmailPattern.IsMatch(email))
        {
            throw new ArgumentException("Author email format is invalid.", nameof(email));
        }
    }

    /// <summary>
    /// Validates content body and throws if invalid.
    /// </summary>
    public static void ValidateContentBody(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content is required.", nameof(content));
        }

        if (content.Length > Comment.ContentBodyMaxLength)
        {
            throw new ArgumentException($"Content cannot exceed {Comment.ContentBodyMaxLength} characters.", nameof(content));
        }
    }

    /// <summary>
    /// Sanitizes author name: strips any HTML tags, trims, and enforces max length.
    /// </summary>
    public static string SanitizeAuthorName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return name;
        }

        var stripped = PlainTextSanitizer.Sanitize(name);
        var trimmed = stripped.Trim();
        if (trimmed.Length > Comment.AuthorNameMaxLength)
        {
            trimmed = trimmed[..Comment.AuthorNameMaxLength];
        }

        return trimmed;
    }

    /// <summary>
    /// Sanitizes author email: strips any HTML tags, trims, and enforces max length.
    /// </summary>
    public static string SanitizeAuthorEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return email;
        }

        var stripped = PlainTextSanitizer.Sanitize(email);
        var trimmed = stripped.Trim();
        if (trimmed.Length > Comment.AuthorEmailMaxLength)
        {
            trimmed = trimmed[..Comment.AuthorEmailMaxLength];
        }

        return trimmed;
    }

    /// <summary>
    /// Sanitizes content body using HtmlSanitizer with minimal whitelist (strong, em, a, br).
    /// Result is safe for MarkupString rendering; for defense in depth, plain text display is also safe.
    /// </summary>
    public static string SanitizeContentBody(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return content;
        }

        var sanitized = ContentBodySanitizer.Sanitize(content);
        if (sanitized.Length > Comment.ContentBodyMaxLength)
        {
            sanitized = sanitized[..Comment.ContentBodyMaxLength];
        }

        return sanitized;
    }
}

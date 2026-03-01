using ShireBudgeters.Common.Common.Constants;
using System.Text.RegularExpressions;

namespace ShireBudgeters.BL.Common.Helpers;

/// <summary>
/// Helper for comment validation and sanitization.
/// </summary>
public static class CommentValidationHelper
{
    /// <summary>
    /// Basic email format pattern for server-side validation.
    /// </summary>
    private static readonly Regex EmailPattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

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
    /// Sanitizes author name to prevent XSS.
    /// </summary>
    public static string SanitizeAuthorName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return name;
        }

        return SanitizePlainText(name);
    }

    /// <summary>
    /// Sanitizes author email to prevent XSS.
    /// </summary>
    public static string SanitizeAuthorEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return email;
        }

        return SanitizePlainText(email);
    }

    /// <summary>
    /// Sanitizes content body to prevent XSS (uses same approach as post content).
    /// </summary>
    public static string SanitizeContentBody(string content)
    {
        return PostValidationHelper.SanitizeContentBody(content);
    }

    private static string SanitizePlainText(string text)
    {
        foreach (var pattern in Comment.DangerousXssPatterns)
        {
            if (Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase))
            {
                text = Regex.Replace(text, pattern, string.Empty, RegexOptions.IgnoreCase);
            }
        }

        return text;
    }
}

using ShireBudgeters.Common.Common.Constants;
using System.Text.RegularExpressions;

namespace ShireBudgeters.BL.Common.Helpers;

/// <summary>
/// Helper class for lead magnet validation and security features.
/// </summary>
/// <remarks>Provides validation methods for lead magnet titles, URLs, and category IDs
/// to prevent XSS attacks, URL injection, and ensure data integrity.</remarks>
public static class LeadMagnetValidationHelper
{
    /// <summary>
    /// Validates that a lead magnet title is not null or empty and does not exceed the maximum length.
    /// </summary>
    /// <param name="title">The lead magnet title to validate.</param>
    /// <returns>True if the title is valid, false otherwise.</returns>
    public static bool IsValidTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return false;
        }

        return title.Length <= LeadMagnet.TitleMaxLength;
    }

    /// <summary>
    /// Validates that a lead magnet title is valid and throws an exception if invalid.
    /// </summary>
    /// <param name="title">The lead magnet title to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the title is null, empty, or exceeds the maximum length.</exception>
    public static void ValidateTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Lead magnet title is required.", nameof(title));
        }

        if (title.Length > LeadMagnet.TitleMaxLength)
        {
            throw new ArgumentException($"Lead magnet title cannot exceed {LeadMagnet.TitleMaxLength} characters.", nameof(title));
        }
    }

    /// <summary>
    /// Sanitizes lead magnet title to prevent XSS attacks.
    /// </summary>
    /// <param name="title">The lead magnet title to sanitize.</param>
    /// <returns>The sanitized lead magnet title.</returns>
    public static string SanitizeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return title;
        }

        return SanitizeText(title);
    }

    /// <summary>
    /// Validates that a form action URL is in a valid format and does not exceed the maximum length.
    /// </summary>
    /// <param name="formActionUrl">The form action URL to validate.</param>
    /// <returns>True if the URL is valid, false otherwise.</returns>
    public static bool IsValidFormActionUrl(string? formActionUrl)
    {
        if (string.IsNullOrWhiteSpace(formActionUrl))
        {
            return true; // Empty is valid (optional field)
        }

        if (formActionUrl.Length > LeadMagnet.FormActionUrlMaxLength)
        {
            return false;
        }

        return IsValidUrlFormat(formActionUrl);
    }

    /// <summary>
    /// Validates that a form action URL is valid and throws an exception if invalid.
    /// </summary>
    /// <param name="formActionUrl">The form action URL to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the URL is invalid or exceeds the maximum length.</exception>
    public static void ValidateFormActionUrl(string? formActionUrl)
    {
        if (string.IsNullOrWhiteSpace(formActionUrl))
        {
            return; // Optional field
        }

        if (formActionUrl.Length > LeadMagnet.FormActionUrlMaxLength)
        {
            throw new ArgumentException($"Form action URL cannot exceed {LeadMagnet.FormActionUrlMaxLength} characters.", nameof(formActionUrl));
        }

        if (!IsValidUrlFormat(formActionUrl))
        {
            throw new ArgumentException("Form action URL must be a valid absolute URL (http:// or https://).", nameof(formActionUrl));
        }
    }

    /// <summary>
    /// Validates that a download file URL is in a valid format and does not exceed the maximum length.
    /// </summary>
    /// <param name="downloadFileUrl">The download file URL to validate.</param>
    /// <returns>True if the URL is valid, false otherwise.</returns>
    public static bool IsValidDownloadFileUrl(string? downloadFileUrl)
    {
        if (string.IsNullOrWhiteSpace(downloadFileUrl))
        {
            return true; // Empty is valid (optional field)
        }

        if (downloadFileUrl.Length > LeadMagnet.DownloadFileUrlMaxLength)
        {
            return false;
        }

        return IsValidUrlFormat(downloadFileUrl);
    }

    /// <summary>
    /// Validates that a download file URL is valid and throws an exception if invalid.
    /// </summary>
    /// <param name="downloadFileUrl">The download file URL to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the URL is invalid or exceeds the maximum length.</exception>
    public static void ValidateDownloadFileUrl(string? downloadFileUrl)
    {
        if (string.IsNullOrWhiteSpace(downloadFileUrl))
        {
            return; // Optional field
        }

        if (downloadFileUrl.Length > LeadMagnet.DownloadFileUrlMaxLength)
        {
            throw new ArgumentException($"Download file URL cannot exceed {LeadMagnet.DownloadFileUrlMaxLength} characters.", nameof(downloadFileUrl));
        }

        if (!IsValidUrlFormat(downloadFileUrl))
        {
            throw new ArgumentException("Download file URL must be a valid absolute URL (http:// or https://).", nameof(downloadFileUrl));
        }
    }

    /// <summary>
    /// Validates that a category ID is greater than zero.
    /// </summary>
    /// <param name="categoryId">The category ID to validate.</param>
    /// <returns>True if the category ID is valid, false otherwise.</returns>
    public static bool IsValidCategoryId(int categoryId)
    {
        return categoryId > 0;
    }

    /// <summary>
    /// Validates that a category ID is valid and throws an exception if invalid.
    /// </summary>
    /// <param name="categoryId">The category ID to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the category ID is invalid.</exception>
    public static void ValidateCategoryId(int categoryId)
    {
        if (categoryId <= 0)
        {
            throw new ArgumentException("Category ID must be greater than zero.", nameof(categoryId));
        }
    }

    /// <summary>
    /// Validates that a URL is in a valid format (absolute URL with http:// or https://).
    /// </summary>
    /// <param name="url">The URL to validate.</param>
    /// <returns>True if the URL format is valid, false otherwise.</returns>
    private static bool IsValidUrlFormat(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        // Use Uri.TryCreate to validate URL format
        if (Uri.TryCreate(url, UriKind.Absolute, out var result))
        {
            // Ensure it's http or https
            return result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps;
        }

        return false;
    }

    /// <summary>
    /// Sanitizes text content to prevent XSS attacks by removing dangerous patterns.
    /// </summary>
    /// <param name="text">The text to sanitize.</param>
    /// <returns>The sanitized text.</returns>
    private static string SanitizeText(string text)
    {
        foreach (var pattern in LeadMagnet.DangerousXssPatterns)
        {
            if (Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase))
            {
                // Remove dangerous content
                text = Regex.Replace(text, pattern, string.Empty, RegexOptions.IgnoreCase);
            }
        }

        return text;
    }
}


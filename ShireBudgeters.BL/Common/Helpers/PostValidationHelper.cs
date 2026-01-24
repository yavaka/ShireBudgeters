using ShireBudgeters.Common.Common.Constants;
using System.Text.RegularExpressions;

namespace ShireBudgeters.BL.Common.Helpers;

/// <summary>
/// Helper class for post validation and security features.
/// </summary>
/// <remarks>Provides validation methods for slugs, content sanitization, image URLs, and meta descriptions
/// to prevent XSS attacks, URL injection, and ensure data integrity.</remarks>
public static class PostValidationHelper
{

    /// <summary>
    /// Validates that a slug is URL-friendly (lowercase, alphanumeric characters and hyphens only).
    /// </summary>
    /// <param name="slug">The slug to validate.</param>
    /// <returns>True if the slug is valid, false otherwise.</returns>
    public static bool IsValidSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return false;
        }

        // Slug should be lowercase
        if (slug != slug.ToLowerInvariant())
        {
            return false;
        }

        // Slug should only contain lowercase letters, numbers, and hyphens
        // Should not start or end with a hyphen
        return Regex.IsMatch(slug, Post.SlugPattern);
    }

    /// <summary>
    /// Checks if a slug contains patterns that could be used for URL injection attacks.
    /// </summary>
    /// <param name="slug">The slug to check.</param>
    /// <returns>True if dangerous patterns are found, false otherwise.</returns>
    public static bool ContainsUrlInjectionPatterns(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return false;
        }

        return Post.DangerousUrlPatterns.Any(pattern => Regex.IsMatch(slug, pattern, RegexOptions.IgnoreCase));
    }

    /// <summary>
    /// Validates that a slug is both URL-friendly and doesn't contain injection patterns.
    /// </summary>
    /// <param name="slug">The slug to validate.</param>
    /// <returns>True if the slug is valid and safe, false otherwise.</returns>
    public static bool ValidateSlug(string slug) 
        => IsValidSlug(slug) && !ContainsUrlInjectionPatterns(slug);

    /// <summary>
    /// Sanitizes ContentBody to prevent XSS attacks.
    /// </summary>
    /// <param name="contentBody">The content body to sanitize.</param>
    /// <returns>The sanitized content body.</returns>
    /// <remarks>
    /// Note: For production use, consider using a library like HtmlSanitizer (Ganss.Xss) for more comprehensive sanitization.
    /// This method removes dangerous patterns but for a blog system that needs to support HTML content, you should use
    /// a proper HTML sanitizer library such as Ganss.Xss.HtmlSanitizer which allows safe HTML tags while removing dangerous ones.
    /// </remarks>
    public static string SanitizeContentBody(string contentBody)
    {
        if (string.IsNullOrWhiteSpace(contentBody))
        {
            return contentBody;
        }

        foreach (var pattern in Post.DangerousXssPatterns)
        {
            if (Regex.IsMatch(contentBody, pattern, RegexOptions.IgnoreCase))
            {
                // Remove dangerous content
                contentBody = Regex.Replace(contentBody, pattern, string.Empty, RegexOptions.IgnoreCase);
            }
        }

        // TODO: For production, implement proper HTML sanitization using a library like:
        // - Ganss.Xss.HtmlSanitizer (recommended)
        // - AngleSharp with custom sanitization rules
        // This will allow safe HTML tags (p, div, strong, em, etc.) while removing dangerous ones

        return contentBody;
    }

    /// <summary>
    /// Validates that a FeaturedImageUrl points to an allowed domain or relative path.
    /// </summary>
    /// <param name="imageUrl">The image URL to validate.</param>
    /// <returns>True if the URL is valid and allowed, false otherwise.</returns>
    public static bool IsValidImageUrl(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return false;
        }

        // Relative paths are always allowed (starting with /)
        if (imageUrl.StartsWith("/", StringComparison.Ordinal))
        {
            // Validate that it's a valid relative path (not path traversal)
            if (imageUrl.Contains(Post.PathTraversalPattern, StringComparison.Ordinal))
            {
                return false;
            }

            // Check if it matches allowed paths (optional, for stricter control)
            if (Post.AllowedImagePaths.Length > 0 && !Post.AllowedImagePaths.Any(path => imageUrl.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
            {
                // Allow any relative path that doesn't contain path traversal
                // You can make this stricter by requiring paths to match AllowedImagePaths
            }

            return true;
        }

        // For absolute URLs, validate the domain
        if (Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri))
        {
            // Only allow HTTP and HTTPS
            if (uri.Scheme != Post.HttpScheme && uri.Scheme != Post.HttpsScheme)
            {
                return false;
            }

            // Check if domain is in allowed list
            var host = uri.Host.ToLowerInvariant();
            
            // Remove port if present
            var hostWithoutPort = host.Split(':')[0];

            if (Post.AllowedImageDomains.Length > 0)
            {
                return Post.AllowedImageDomains.Any(domain => 
                    hostWithoutPort == domain.ToLowerInvariant() || 
                    hostWithoutPort.EndsWith($".{domain.ToLowerInvariant()}", StringComparison.OrdinalIgnoreCase));
            }

            // If no allowed domains configured, allow all HTTPS URLs (less secure, but flexible)
            // In production, you should configure AllowedImageDomains
            return uri.Scheme == Post.HttpsScheme;
        }

        return false;
    }

    /// <summary>
    /// Validates that a MetaDescription does not exceed the maximum allowed length.
    /// </summary>
    /// <param name="metaDescription">The meta description to validate.</param>
    /// <param name="maxLength">The maximum allowed length (default: Post.MetaDescriptionMaxLength).</param>
    /// <returns>True if the meta description is valid, false otherwise.</returns>
    public static bool IsValidMetaDescription(string? metaDescription, int maxLength = Post.MetaDescriptionMaxLength)
    {
        if (string.IsNullOrWhiteSpace(metaDescription))
        {
            return true; // Empty is valid
        }

        return metaDescription.Length <= maxLength;
    }

    /// <summary>
    /// Validates that a MetaDescription does not exceed the maximum allowed length and throws an exception if invalid.
    /// </summary>
    /// <param name="metaDescription">The meta description to validate.</param>
    /// <param name="maxLength">The maximum allowed length (default: Post.MetaDescriptionMaxLength).</param>
    /// <exception cref="ArgumentException">Thrown when the meta description exceeds the maximum length.</exception>
    public static void ValidateMetaDescription(string? metaDescription, int maxLength = Post.MetaDescriptionMaxLength)
    {
        if (!IsValidMetaDescription(metaDescription, maxLength))
        {
            throw new ArgumentException($"MetaDescription cannot exceed {maxLength} characters.", nameof(metaDescription));
        }
    }
}


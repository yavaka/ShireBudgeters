using ShireBudgeters.Common.Common.Constants;
using System.Text.RegularExpressions;

namespace ShireBudgeters.BL.Common.Helpers;

/// <summary>
/// Helper class for category validation and security features.
/// </summary>
/// <remarks>Provides validation methods for category names, descriptions, colors, and user IDs
/// to prevent XSS attacks and ensure data integrity. Also includes helpers for hierarchy validation.</remarks>
public static class CategoryValidationHelper
{
    /// <summary>
    /// Validates that a category name is not null or empty and does not exceed the maximum length.
    /// </summary>
    /// <param name="name">The category name to validate.</param>
    /// <returns>True if the name is valid, false otherwise.</returns>
    public static bool IsValidName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        return name.Length <= Category.NameMaxLength;
    }

    /// <summary>
    /// Validates that a category name is valid and throws an exception if invalid.
    /// </summary>
    /// <param name="name">The category name to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the name is null, empty, or exceeds the maximum length.</exception>
    public static void ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name is required.", nameof(name));
        }

        if (name.Length > Category.NameMaxLength)
        {
            throw new ArgumentException($"Category name cannot exceed {Category.NameMaxLength} characters.", nameof(name));
        }
    }

    /// <summary>
    /// Validates that a category description does not exceed the maximum length.
    /// </summary>
    /// <param name="description">The category description to validate.</param>
    /// <returns>True if the description is valid, false otherwise.</returns>
    public static bool IsValidDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return true; // Empty is valid
        }

        return description.Length <= Category.DescriptionMaxLength;
    }

    /// <summary>
    /// Validates that a category description does not exceed the maximum length and throws an exception if invalid.
    /// </summary>
    /// <param name="description">The category description to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the description exceeds the maximum length.</exception>
    public static void ValidateDescription(string? description)
    {
        if (!IsValidDescription(description))
        {
            throw new ArgumentException($"Category description cannot exceed {Category.DescriptionMaxLength} characters.", nameof(description));
        }
    }

    /// <summary>
    /// Validates that a category color does not exceed the maximum length.
    /// </summary>
    /// <param name="color">The category color to validate.</param>
    /// <returns>True if the color is valid, false otherwise.</returns>
    public static bool IsValidColorLength(string? color)
    {
        if (string.IsNullOrWhiteSpace(color))
        {
            return true; // Empty is valid
        }

        return color.Length <= Category.ColorMaxLength;
    }

    /// <summary>
    /// Validates that a category color does not exceed the maximum length and throws an exception if invalid.
    /// </summary>
    /// <param name="color">The category color to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the color exceeds the maximum length.</exception>
    public static void ValidateColorLength(string? color)
    {
        if (!IsValidColorLength(color))
        {
            throw new ArgumentException($"Category color cannot exceed {Category.ColorMaxLength} characters.", nameof(color));
        }
    }

    /// <summary>
    /// Validates that a color value is in a valid format (hex color or CSS color name).
    /// </summary>
    /// <param name="color">The color value to validate.</param>
    /// <returns>True if the color format is valid, false otherwise.</returns>
    /// <remarks>
    /// Validates hex colors (#FF0000, #fff) and basic CSS color name format.
    /// For production, consider using a comprehensive CSS color name list.
    /// </remarks>
    public static bool IsValidColorFormat(string? color)
    {
        if (string.IsNullOrWhiteSpace(color))
        {
            return true; // Empty is valid
        }

        // Check hex color format
        if (Regex.IsMatch(color, Category.HexColorPattern))
        {
            return true;
        }

        // Check basic CSS color name format (simplified - would need full list for complete validation)
        if (Regex.IsMatch(color, Category.CssColorNamePattern))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Validates that a user ID is not null or empty and does not exceed the maximum length.
    /// </summary>
    /// <param name="userId">The user ID to validate.</param>
    /// <returns>True if the user ID is valid, false otherwise.</returns>
    public static bool IsValidUserId(string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return false;
        }

        return userId.Length <= Category.UserIdMaxLength;
    }

    /// <summary>
    /// Validates that a user ID is valid and throws an exception if invalid.
    /// </summary>
    /// <param name="userId">The user ID to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the user ID is null, empty, or exceeds the maximum length.</exception>
    public static void ValidateUserId(string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (userId.Length > Category.UserIdMaxLength)
        {
            throw new ArgumentException($"UserId cannot exceed {Category.UserIdMaxLength} characters.", nameof(userId));
        }
    }

    /// <summary>
    /// Sanitizes category name to prevent XSS attacks.
    /// </summary>
    /// <param name="name">The category name to sanitize.</param>
    /// <returns>The sanitized category name.</returns>
    public static string SanitizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return name;
        }

        return SanitizeText(name);
    }

    /// <summary>
    /// Sanitizes category description to prevent XSS attacks.
    /// </summary>
    /// <param name="description">The category description to sanitize.</param>
    /// <returns>The sanitized category description.</returns>
    public static string? SanitizeDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return description;
        }

        return SanitizeText(description);
    }

    /// <summary>
    /// Validates that a category cannot be its own parent (prevents self-reference).
    /// </summary>
    /// <param name="categoryId">The category ID.</param>
    /// <param name="parentCategoryId">The parent category ID to validate.</param>
    /// <returns>True if valid (not self-referencing), false otherwise.</returns>
    public static bool IsValidParentCategory(int categoryId, int? parentCategoryId)
    {
        if (!parentCategoryId.HasValue)
        {
            return true; // No parent is valid
        }

        return parentCategoryId.Value != categoryId;
    }

    /// <summary>
    /// Validates that a category cannot be its own parent and throws an exception if invalid.
    /// </summary>
    /// <param name="categoryId">The category ID.</param>
    /// <param name="parentCategoryId">The parent category ID to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the category is set as its own parent.</exception>
    public static void ValidateParentCategory(int categoryId, int? parentCategoryId)
    {
        if (!IsValidParentCategory(categoryId, parentCategoryId))
        {
            throw new ArgumentException("A category cannot be its own parent.", nameof(parentCategoryId));
        }
    }

    /// <summary>
    /// Validates ownership by checking if the user ID matches the category's user ID.
    /// </summary>
    /// <param name="categoryUserId">The user ID of the category.</param>
    /// <param name="requestedUserId">The user ID from the request.</param>
    /// <returns>True if ownership is valid, false otherwise.</returns>
    public static bool ValidateOwnership(string categoryUserId, string requestedUserId)
    {
        return categoryUserId == requestedUserId;
    }

    /// <summary>
    /// Validates ownership and throws an exception if invalid.
    /// </summary>
    /// <param name="categoryUserId">The user ID of the category.</param>
    /// <param name="requestedUserId">The user ID from the request.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when ownership validation fails.</exception>
    public static void ValidateOwnershipOrThrow(string categoryUserId, string requestedUserId)
    {
        if (!ValidateOwnership(categoryUserId, requestedUserId))
        {
            throw new UnauthorizedAccessException("Category does not belong to the specified user.");
        }
    }

    /// <summary>
    /// Sanitizes text content to prevent XSS attacks by removing dangerous patterns.
    /// </summary>
    /// <param name="text">The text to sanitize.</param>
    /// <returns>The sanitized text.</returns>
    /// <remarks>
    /// Note: For production use, consider using a library like HtmlSanitizer (Ganss.Xss) for more comprehensive sanitization.
    /// This method removes dangerous patterns but for content that needs to support HTML, you should use
    /// a proper HTML sanitizer library such as Ganss.Xss.HtmlSanitizer which allows safe HTML tags while removing dangerous ones.
    /// </remarks>
    private static string SanitizeText(string text)
    {
        foreach (var pattern in Category.DangerousXssPatterns)
        {
            if (Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase))
            {
                // Remove dangerous content
                text = Regex.Replace(text, pattern, string.Empty, RegexOptions.IgnoreCase);
            }
        }

        // TODO: For production, implement proper HTML sanitization using a library like:
        // - Ganss.Xss.HtmlSanitizer (recommended)
        // - AngleSharp with custom sanitization rules
        // This will allow safe HTML tags (p, div, strong, em, etc.) while removing dangerous ones

        return text;
    }
}


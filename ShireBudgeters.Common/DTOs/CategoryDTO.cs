namespace ShireBudgeters.Common.DTOs;

/// <summary>
/// Data Transfer Object for category information.
/// </summary>
/// <remarks>Represents a category that can be organized hierarchically and associated with a user.
/// Includes all category properties and audit information for data transfer between layers.</remarks>
public class CategoryDTO
{
    /// <summary>
    /// Gets or sets the unique identifier for the category.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the URL-friendly slug for the category (e.g. "finance", "finance/investing").
    /// Used for public routes and navbar; must be unique when set.
    /// </summary>
    public string? Slug { get; set; }

    /// <summary>
    /// Gets or sets the description of the category.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the color or icon identifier for the category.
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who owns this category.
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the parent category (for hierarchical categories).
    /// </summary>
    public int? ParentCategoryId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the category is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the identifier of the user who created the record.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified the record.
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }
}

using ShireBudgeters.DA.Common.Audit;

namespace ShireBudgeters.DA.Models;

/// <summary>
/// Represents a category that can be organized hierarchically and associated with a user.
/// </summary>
/// <remarks>A category can have a parent category and multiple child categories, enabling hierarchical
/// organization. Each category is owned by a user and may include additional metadata such as a description or color.
/// Inherits audit properties from AuditableModel.</remarks>
public class CategoryModel : AuditableModel
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

    // Navigation Properties
    /// <summary>
    /// Gets or sets the user who owns this category.
    /// </summary>
    public virtual UserModel? User { get; set; }

    /// <summary>
    /// Gets or sets the parent category.
    /// </summary>
    public virtual CategoryModel? ParentCategory { get; set; }

    /// <summary>
    /// Gets or sets the child categories.
    /// </summary>
    public virtual ICollection<CategoryModel> ChildCategories { get; set; } = new List<CategoryModel>();
}

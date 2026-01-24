using ShireBudgeters.Common.DTOs;

namespace ShireBudgeters.BL.Services.Category;

/// <summary>
/// Service interface for category operations.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Gets a category by its identifier.
    /// </summary>
    Task<CategoryDTO?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all categories for a specific user.
    /// </summary>
    Task<IEnumerable<CategoryDTO>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active categories for a specific user.
    /// </summary>
    Task<IEnumerable<CategoryDTO>> GetActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all root categories (categories without a parent) for a specific user.
    /// </summary>
    Task<IEnumerable<CategoryDTO>> GetRootCategoriesByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all child categories for a specific parent category.
    /// </summary>
    Task<IEnumerable<CategoryDTO>> GetChildCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new category.
    /// </summary>
    Task<CategoryDTO> CreateAsync(CategoryDTO categoryDto, string? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    Task<CategoryDTO> UpdateAsync(CategoryDTO categoryDto, string? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a category permanently.
    /// </summary>
    /// <param name="id">The identifier of the category to delete.</param>
    /// <param name="userId">Optional user identifier. If provided, verifies ownership before deletion.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task DeleteAsync(int id, string? userId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes a category by setting IsActive to false.
    /// </summary>
    Task SoftDeleteAsync(int id, string? userId, CancellationToken cancellationToken = default);
}


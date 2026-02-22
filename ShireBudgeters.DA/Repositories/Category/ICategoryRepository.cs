using ShireBudgeters.Common;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Repositories.Category;

/// <summary>
/// Repository interface for category data access operations.
/// </summary>
/// <remarks>Extends the base repository interface to provide standard CRUD operations
/// and adds domain-specific methods for querying categories by user, hierarchy, and active status.</remarks>
public interface ICategoryRepository : IRepository<CategoryModel, int>
{
    /// <summary>
    /// Retrieves all categories (active and inactive) for a specific user.
    /// </summary>
    /// <param name="userId">The identifier of the user who owns the categories.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of categories owned by the specified user, including parent and child category relationships.</returns>
    /// <remarks>Used for user dashboard and admin views. Returns both active and inactive categories.</remarks>
    Task<IEnumerable<CategoryModel>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a category by its URL slug (e.g. "finance", "finance/investing").
    /// </summary>
    /// <param name="slug">The unique slug of the category.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The category with the specified slug, or null if not found.</returns>
    Task<CategoryModel?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all active categories (no user filter). Used for public navbar and category pages.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>All active categories ordered by name.</returns>
    Task<IEnumerable<CategoryModel>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves only active categories for a specific user.
    /// </summary>
    /// <param name="userId">The identifier of the user who owns the categories.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of active categories owned by the specified user, including parent category relationships.</returns>
    /// <remarks>Used for displaying categories in user interfaces where only active categories should be shown.</remarks>
    Task<IEnumerable<CategoryModel>> GetActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves root categories (categories without a parent) for a specific user.
    /// </summary>
    /// <param name="userId">The identifier of the user who owns the categories.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of root categories owned by the specified user, including their child categories.</returns>
    /// <remarks>Used for building hierarchical category trees. Root categories are those with ParentCategoryId set to null.</remarks>
    Task<IEnumerable<CategoryModel>> GetRootCategoriesByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all child categories for a specific parent category.
    /// </summary>
    /// <param name="parentCategoryId">The identifier of the parent category.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of child categories that belong to the specified parent category.</returns>
    /// <remarks>Used for navigating category hierarchies and displaying subcategories.</remarks>
    Task<IEnumerable<CategoryModel>> GetChildCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the number of blog posts and lead magnets that reference the specified category.
    /// </summary>
    /// <param name="categoryId">The category identifier.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A tuple of (post count, lead magnet count) that reference the category.</returns>
    /// <remarks>Used to prevent deleting a category that is still in use.</remarks>
    Task<(int PostCount, int LeadMagnetCount)> GetDependentCountsAsync(int categoryId, CancellationToken cancellationToken = default);
}
using ShireBudgeters.Common;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Repositories.Post;

/// <summary>
/// Repository interface for blog post data access operations.
/// </summary>
/// <remarks>Extends the base repository interface to provide standard CRUD operations
/// and adds domain-specific methods for querying blog posts by various criteria.</remarks>
public interface IPostRepository : IRepository<PostModel, int>
{
    /// <summary>
    /// Retrieves a post by its unique slug (for URL routing).
    /// </summary>
    /// <param name="slug">The URL-friendly slug of the post.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The post with the specified slug, or null if not found.</returns>
    /// <remarks>Used for public-facing blog post retrieval via URL-friendly slugs.</remarks>
    Task<PostModel?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all published posts, ordered by publication date (newest first).
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of published posts where IsPublished is true and PublicationDate is less than or equal to the current UTC time.</returns>
    /// <remarks>Used for public blog listings. Only returns posts that are currently visible (published and past their publication date).</remarks>
    Task<IEnumerable<PostModel>> GetPublishedPostsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves published posts filtered by category.
    /// </summary>
    /// <param name="categoryId">The identifier of the category to filter by.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of published posts in the specified category, ordered by publication date (newest first).</returns>
    /// <remarks>Used for category-specific blog listings. Includes the same published filters as GetPublishedPostsAsync.</remarks>
    Task<IEnumerable<PostModel>> GetPublishedPostsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a limited number of recent published posts in a category.
    /// </summary>
    /// <param name="categoryId">The identifier of the category to filter by.</param>
    /// <param name="count">The maximum number of posts to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of recent published posts in the specified category, ordered by publication date descending and limited to the specified count.</returns>
    /// <remarks>Used for homepage thematic blocks. Same filters as GetPublishedPostsByCategoryAsync with Take(count).</remarks>
    Task<IEnumerable<PostModel>> GetRecentPublishedPostsByCategoryAsync(int categoryId, int count, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves published posts filtered by any of the given category IDs, ordered by publication date (newest first).
    /// </summary>
    /// <param name="categoryIds">The category identifiers to filter by (e.g. parent + child category IDs).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of published posts in any of the specified categories, ordered by publication date descending.</returns>
    /// <remarks>Used for parent category pages that show posts from the parent and all subcategories.</remarks>
    Task<IEnumerable<PostModel>> GetPublishedPostsByCategoryIdsAsync(IEnumerable<int> categoryIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all posts (published and unpublished) for a specific author.
    /// </summary>
    /// <param name="authorId">The identifier of the author.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of all posts by the specified author, ordered by publication date descending.</returns>
    /// <remarks>Used for author dashboard and admin views. Returns both drafts and published posts.</remarks>
    Task<IEnumerable<PostModel>> GetByAuthorIdAsync(string authorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves only draft posts (unpublished) for a specific author.
    /// </summary>
    /// <param name="authorId">The identifier of the author.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of draft posts by the specified author, ordered by creation date descending.</returns>
    /// <remarks>Used for author's draft management interface.</remarks>
    Task<IEnumerable<PostModel>> GetDraftsByAuthorIdAsync(string authorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a limited number of recent published posts.
    /// </summary>
    /// <param name="count">The maximum number of posts to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of recent published posts, ordered by publication date descending and limited to the specified count.</returns>
    /// <remarks>Used for homepage or sidebar "recent posts" widgets. Orders by PublicationDate descending and limits results.</remarks>
    Task<IEnumerable<PostModel>> GetRecentPublishedPostsAsync(int count, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a slug already exists (for validation before creating/updating posts).
    /// </summary>
    /// <param name="slug">The slug to check for uniqueness.</param>
    /// <param name="excludePostId">Optional post ID to exclude from the check (useful when updating an existing post).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>True if the slug exists (excluding the specified post ID if provided), false otherwise.</returns>
    /// <remarks>The excludePostId parameter allows checking uniqueness while excluding the current post during updates.</remarks>
    Task<bool> CheckSlugExistsAsync(string slug, int? excludePostId = null, CancellationToken cancellationToken = default);
}
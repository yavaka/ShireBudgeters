using ShireBudgeters.Common.DTOs;

namespace ShireBudgeters.BL.Services.Post;

/// <summary>
/// Service interface for blog post operations.
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Retrieves a post by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the post.</param>
    /// <param name="userId">Optional user identifier. If provided and the post is a draft, only returns the post if the user is the author.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The post if found and accessible, null otherwise.</returns>
    /// <remarks>Draft posts are only returned to their authors. Published posts are accessible to everyone.</remarks>
    Task<PostDTO?> GetByIdAsync(int id, string? userId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a published post by its slug.
    /// </summary>
    Task<PostDTO?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all published posts, ordered by publication date (newest first).
    /// </summary>
    Task<IEnumerable<PostDTO>> GetPublishedPostsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves published posts filtered by category.
    /// </summary>
    Task<IEnumerable<PostDTO>> GetPublishedPostsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a limited number of recent published posts in a category.
    /// </summary>
    Task<IEnumerable<PostDTO>> GetRecentPublishedPostsByCategoryAsync(int categoryId, int count, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves published posts for a parent category and all its child categories, ordered by publication date (newest first).
    /// </summary>
    /// <param name="parentCategoryId">The parent category identifier.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Published posts in the parent or any of its subcategories, ordered by publication date descending.</returns>
    Task<IEnumerable<PostDTO>> GetPublishedPostsByCategoryAndDescendantsAsync(int parentCategoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all posts (published and unpublished) for an author.
    /// </summary>
    Task<IEnumerable<PostDTO>> GetByAuthorIdAsync(string authorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves draft posts for an author.
    /// </summary>
    Task<IEnumerable<PostDTO>> GetDraftsByAuthorIdAsync(string authorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a limited number of recent published posts.
    /// </summary>
    Task<IEnumerable<PostDTO>> GetRecentPublishedPostsAsync(int count, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches published posts by title, slug, meta description, or content body (case-insensitive).
    /// </summary>
    /// <param name="searchTerm">The search term. If null or whitespace, returns an empty list.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>Matching published posts, ordered by publication date (newest first), limited to a maximum number of results.</returns>
    Task<IEnumerable<PostDTO>> SearchPublishedPostsAsync(string? searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new blog post.
    /// </summary>
    Task<PostDTO> CreateAsync(PostDTO postDto, string? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing blog post.
    /// </summary>
    Task<PostDTO> UpdateAsync(PostDTO postDto, string? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes a blog post.
    /// </summary>
    Task DeleteAsync(int id, string? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a draft post.
    /// </summary>
    Task<PostDTO> PublishAsync(int id, string? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unpublishes a post (converts to draft).
    /// </summary>
    Task<PostDTO> UnpublishAsync(int id, string? userId, CancellationToken cancellationToken = default);
}


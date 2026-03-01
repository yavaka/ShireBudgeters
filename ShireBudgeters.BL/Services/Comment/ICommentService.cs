using ShireBudgeters.Common.DTOs;

namespace ShireBudgeters.BL.Services.Comment;

/// <summary>
/// Service interface for comment operations.
/// </summary>
public interface ICommentService
{
    /// <summary>
    /// Retrieves all comments for a post, ordered by creation date (oldest first).
    /// </summary>
    /// <param name="postId">The identifier of the post.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<IEnumerable<CommentDTO>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a comment by its identifier (admin use).
    /// </summary>
    /// <param name="id">The identifier of the comment.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<CommentDTO?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="dto">The comment data.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The created comment.</returns>
    Task<CommentDTO> CreateAsync(CreateCommentDTO dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing comment (admin use).
    /// </summary>
    /// <param name="dto">The updated comment data.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The updated comment.</returns>
    Task<CommentDTO> UpdateAsync(CommentDTO dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a comment (admin use).
    /// </summary>
    /// <param name="id">The identifier of the comment to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}

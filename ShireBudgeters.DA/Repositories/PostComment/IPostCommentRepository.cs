using ShireBudgeters.Common;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Repositories.PostComment;

public interface IPostCommentRepository : IRepository<CommentModel, int>
{
    /// <summary>
    /// Retrieves all comments for a post, ordered by creation date (oldest first).
    /// </summary>
    /// <param name="postId">The identifier of the post.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<IEnumerable<CommentModel>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default);
}

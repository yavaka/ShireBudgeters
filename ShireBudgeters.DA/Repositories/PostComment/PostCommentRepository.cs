using Microsoft.EntityFrameworkCore;
using ShireBudgeters.DA.Configurations.Database;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Repositories.PostComment;

/// <summary>
/// Repository implementation for post comment data access operations.
/// </summary>
internal class PostCommentRepository(ShireBudgetersDbContext context) : Repository<CommentModel, int>(context), IPostCommentRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<CommentModel>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default) 
        => await _dbSet
            .AsNoTracking()
            .Where(c => c.PostId == postId)
            .OrderBy(c => c.CreatedDate)
            .ToListAsync(cancellationToken);
}

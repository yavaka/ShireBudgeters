using Microsoft.EntityFrameworkCore;
using ShireBudgeters.DA.Configurations.Database;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Repositories.Post;

/// <summary>
/// Repository implementation for blog post data access operations.
/// </summary>
/// <remarks>Provides data access methods for querying and managing blog posts using Entity Framework Core.
/// All read operations use AsNoTracking() for performance optimization.</remarks>
internal class PostRepository(ShireBudgetersDbContext context) : Repository<PostModel, int>(context), IPostRepository
{
    /// <inheritdoc/>
    public override async Task<PostModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Author)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<PostModel?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Author)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostModel>> GetPublishedPostsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Where(p => p.IsPublished && p.PublicationDate <= DateTime.UtcNow)
            .OrderByDescending(p => p.PublicationDate)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostModel>> GetPublishedPostsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Where(p => p.IsPublished && p.PublicationDate <= DateTime.UtcNow && p.CategoryId == categoryId)
            .OrderByDescending(p => p.PublicationDate)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostModel>> GetRecentPublishedPostsByCategoryAsync(int categoryId, int count, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Where(p => p.IsPublished && p.PublicationDate <= DateTime.UtcNow && p.CategoryId == categoryId)
            .OrderByDescending(p => p.PublicationDate)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostModel>> GetPublishedPostsByCategoryIdsAsync(IEnumerable<int> categoryIds, CancellationToken cancellationToken = default)
    {
        var idList = categoryIds.ToList();
        if (idList.Count == 0)
        {
            return [];
        }

        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Where(p => p.IsPublished && p.PublicationDate <= DateTime.UtcNow && p.CategoryId.HasValue && idList.Contains(p.CategoryId.Value))
            .OrderByDescending(p => p.PublicationDate)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostModel>> GetByAuthorIdAsync(string authorId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Where(p => p.AuthorId == authorId)
            .OrderByDescending(p => p.PublicationDate)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostModel>> GetDraftsByAuthorIdAsync(string authorId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Where(p => p.AuthorId == authorId && !p.IsPublished)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostModel>> GetRecentPublishedPostsAsync(int count, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Where(p => p.IsPublished && p.PublicationDate <= DateTime.UtcNow)
            .OrderByDescending(p => p.PublicationDate)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PostModel>> SearchPublishedPostsAsync(string? searchTerm, int maxResults, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || maxResults <= 0)
        {
            return [];
        }

        var term = searchTerm.Trim().ToLowerInvariant();
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Where(p => p.IsPublished && p.PublicationDate <= DateTime.UtcNow &&
                (p.Title.ToLower().Contains(term) ||
                 p.Slug.ToLower().Contains(term) ||
                 (p.MetaDescription != null && p.MetaDescription.ToLower().Contains(term)) ||
                 (p.ContentBody != null && p.ContentBody.ToLower().Contains(term))))
            .OrderByDescending(p => p.PublicationDate)
            .Take(maxResults)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> CheckSlugExistsAsync(string slug, int? excludePostId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .AsNoTracking()
            .Where(p => p.Slug == slug);

        if (excludePostId.HasValue)
        {
            query = query.Where(p => p.Id != excludePostId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using ShireBudgeters.DA.Configurations.Database;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Repositories.Category;

/// <summary>
/// Repository implementation for category data access operations.
/// </summary>
/// <remarks>Provides data access methods for querying and managing categories using Entity Framework Core.
/// All read operations use AsNoTracking() for performance optimization. Supports hierarchical category structures
/// with parent-child relationships.</remarks>
internal class CategoryRepository(ShireBudgetersDbContext context) : Repository<CategoryModel, int>(context), ICategoryRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryModel>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.ParentCategory)
            .Include(c => c.ChildCategories)
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryModel>> GetActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.ParentCategory)
            .Where(c => c.UserId == userId && c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryModel>> GetRootCategoriesByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.ChildCategories)
            .Where(c => c.UserId == userId && c.ParentCategoryId == null)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryModel>> GetChildCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == parentCategoryId)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task<CategoryModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.ParentCategory)
            .Include(c => c.ChildCategories)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<(int PostCount, int LeadMagnetCount)> GetDependentCountsAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var postCount = await _context.BlogPosts.CountAsync(p => p.CategoryId == categoryId, cancellationToken);
        var leadMagnetCount = await _context.LeadMagnets.CountAsync(lm => lm.CategoryId == categoryId, cancellationToken);
        return (postCount, leadMagnetCount);
    }
}

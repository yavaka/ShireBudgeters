using Microsoft.EntityFrameworkCore;
using ShireBudgeters.DA.Configurations.Database;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Repositories.Category;

internal class CategoryRepository(ShireBudgetersDbContext context) : Repository<CategoryModel, int>(context), ICategoryRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryModel>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .Include(c => c.ParentCategory)
            .Include(c => c.ChildCategories)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryModel>> GetActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.UserId == userId && c.IsActive)
            .Include(c => c.ParentCategory)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryModel>> GetRootCategoriesByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.UserId == userId && c.ParentCategoryId == null)
            .Include(c => c.ChildCategories)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryModel>> GetChildCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == parentCategoryId)
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
}

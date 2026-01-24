using Microsoft.EntityFrameworkCore;
using ShireBudgeters.DA.Configurations.Database;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Repositories.LeadMagnet;

/// <summary>
/// Repository implementation for lead magnet data access operations.
/// </summary>
/// <remarks>Provides data access methods for querying and managing lead magnets using Entity Framework Core.
/// All read operations use AsNoTracking() for performance optimization.</remarks>
internal class LeadMagnetRepository(ShireBudgetersDbContext context) : Repository<LeadMagnetModel, int>(context), ILeadMagnetRepository
{
    /// <inheritdoc/>
    public override async Task<LeadMagnetModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default) 
        => await _dbSet
            .AsNoTracking()
            .Include(lm => lm.Category)
            .FirstOrDefaultAsync(lm => lm.Id == id, cancellationToken);

    /// <inheritdoc/>
    public async Task<IEnumerable<LeadMagnetModel>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default) 
        => await _dbSet
            .AsNoTracking()
            .Include(lm => lm.Category)
            .Where(lm => lm.CategoryId == categoryId)
            .OrderBy(lm => lm.Title)
            .ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<IEnumerable<LeadMagnetModel>> GetActiveByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default) 
        => await _dbSet
            .AsNoTracking()
            .Include(lm => lm.Category)
            .Where(lm => lm.CategoryId == categoryId && lm.IsActive)
            .OrderBy(lm => lm.Title)
            .ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<LeadMagnetModel?> GetByIdWithCategoryAsync(int id, CancellationToken cancellationToken = default) 
        => await _dbSet
            .AsNoTracking()
            .Include(lm => lm.Category)
            .FirstOrDefaultAsync(lm => lm.Id == id, cancellationToken);
}

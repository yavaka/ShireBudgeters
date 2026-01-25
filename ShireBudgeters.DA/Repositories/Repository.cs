using Microsoft.EntityFrameworkCore;
using ShireBudgeters.Common;
using ShireBudgeters.DA.Configurations.Database;
using System.Linq.Expressions;

namespace ShireBudgeters.DA.Repositories;

/// <summary>
/// Base repository implementation using Entity Framework Core.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
/// <remarks>   
/// Initializes a new instance of the <see cref="Repository{T, TKey}"/> class.
/// </remarks>
public class Repository<T, TKey>(ShireBudgetersDbContext context) : IRepository<T, TKey> where T : class
{
    protected readonly ShireBudgetersDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    /// <inheritdoc/>
    public virtual async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id!], cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.Where(predicate).ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <inheritdoc/>
    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var entityType = _context.Model.FindEntityType(typeof(T));
        var keyProperty = entityType?.FindPrimaryKey()?.Properties.FirstOrDefault();
        
        if (keyProperty == null)
        {
            throw new InvalidOperationException($"Entity type {typeof(T).Name} does not have a primary key defined.");
        }
        
        var keyValue = typeof(T).GetProperty(keyProperty.Name)?.GetValue(entity);
        
        // Try to find if entity with same key is already tracked
        var trackedEntity = _context.ChangeTracker.Entries<T>()
            .FirstOrDefault(e => Equals(e.Property(keyProperty.Name).CurrentValue, keyValue));
        
        if (trackedEntity != null)
        {
            // Update the tracked entity's properties (only scalar properties, not navigation)
            trackedEntity.CurrentValues.SetValues(entity);
        }
        else
        {
            // Entity is not tracked, get it from database or attach it
            var existingEntity = await _dbSet.FindAsync([keyValue!]);
            
            if (existingEntity != null)
            {
                // Update existing tracked entity
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                // Entity doesn't exist, attach and mark as modified
                // Clear navigation properties first to avoid tracking conflicts
                var entry = _context.Entry(entity);
                foreach (var navigation in entry.Navigations)
                {
                    if (navigation.IsLoaded)
                    {
                        navigation.CurrentValue = null;
                    }
                }
                
                _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
        }
        
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        // Prefer removing an already-tracked instance (avoids duplicate tracking errors).
        var entityType = _context.Model.FindEntityType(typeof(T));
        var keyProperty = entityType?.FindPrimaryKey()?.Properties.FirstOrDefault();

        if (keyProperty != null)
        {
            var keyValue = typeof(T).GetProperty(keyProperty.Name)?.GetValue(entity);

            var trackedEntity = _context.ChangeTracker.Entries<T>()
                .FirstOrDefault(e => Equals(e.Property(keyProperty.Name).CurrentValue, keyValue));

            if (trackedEntity != null)
            {
                _dbSet.Remove(trackedEntity.Entity);
                await _context.SaveChangesAsync(cancellationToken);
                return;
            }
        }

        // If the entity is detached (common when read via AsNoTracking), ensure we don't attach an object graph.
        // Clearing navigation values prevents EF from trying to track related entities (e.g., UserModel),
        // which can cause "another instance with the key value is already being tracked" exceptions.
        var entry = _context.Entry(entity);
        foreach (var navigation in entry.Navigations)
        {
            navigation.CurrentValue = null;
        }

        if (entry.State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        entry.State = EntityState.Deleted;
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        return entity != null;
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
        => await _dbSet.CountAsync(cancellationToken);
}

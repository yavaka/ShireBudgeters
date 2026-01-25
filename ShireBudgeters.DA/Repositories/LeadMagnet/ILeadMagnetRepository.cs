using ShireBudgeters.Common;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Repositories.LeadMagnet;

/// <summary>
/// Repository interface for lead magnet data access operations.
/// </summary>
/// <remarks>Extends the base repository interface to provide standard CRUD operations
/// and adds domain-specific methods for querying lead magnets by category and active status.</remarks>
public interface ILeadMagnetRepository : IRepository<LeadMagnetModel, int>
{
    /// <summary>
    /// Retrieves all lead magnets (active and inactive) for a specific user.
    /// </summary>
    /// <param name="userId">The identifier of the user who owns the associated categories.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of lead magnets owned by the specified user, including category information.</returns>
    Task<IEnumerable<LeadMagnetModel>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves only active lead magnets for a specific user.
    /// </summary>
    /// <param name="userId">The identifier of the user who owns the associated categories.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of active lead magnets owned by the specified user, including category information.</returns>
    Task<IEnumerable<LeadMagnetModel>> GetActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all lead magnets (active and inactive) associated with a specific category.
    /// </summary>
    /// <param name="categoryId">The identifier of the category.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of lead magnets associated with the specified category, including category information.</returns>
    /// <remarks>Used for admin views and category management. Returns both active and inactive lead magnets.</remarks>
    Task<IEnumerable<LeadMagnetModel>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves only active lead magnets for a specific category.
    /// </summary>
    /// <param name="categoryId">The identifier of the category.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of active lead magnets associated with the specified category, including category information.</returns>
    /// <remarks>Used for public-facing category pages where only active lead magnets should be displayed.</remarks>
    Task<IEnumerable<LeadMagnetModel>> GetActiveByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a lead magnet by ID with its associated category information.
    /// </summary>
    /// <param name="id">The identifier of the lead magnet.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The lead magnet with the specified ID including category information, or null if not found.</returns>
    /// <remarks>Used when detailed lead magnet information including category context is needed.</remarks>
    Task<LeadMagnetModel?> GetByIdWithCategoryAsync(int id, CancellationToken cancellationToken = default);
}

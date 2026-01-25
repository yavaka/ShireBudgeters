using ShireBudgeters.Common.DTOs;

namespace ShireBudgeters.BL.Services.LeadMagnet;

/// <summary>
/// Service interface for lead magnet operations.
/// </summary>
public interface ILeadMagnetService
{
    /// <summary>
    /// Gets a lead magnet by its identifier.
    /// </summary>
    Task<LeadMagnetDTO?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all lead magnets (active and inactive) owned by a specific user.
    /// </summary>
    Task<IEnumerable<LeadMagnetDTO>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active lead magnets owned by a specific user.
    /// </summary>
    Task<IEnumerable<LeadMagnetDTO>> GetActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all lead magnets (active and inactive) for a specific category.
    /// </summary>
    Task<IEnumerable<LeadMagnetDTO>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active lead magnets for a specific category.
    /// </summary>
    Task<IEnumerable<LeadMagnetDTO>> GetActiveByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new lead magnet.
    /// </summary>
    Task<LeadMagnetDTO> CreateAsync(LeadMagnetDTO leadMagnetDto, string? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing lead magnet.
    /// </summary>
    Task<LeadMagnetDTO> UpdateAsync(LeadMagnetDTO leadMagnetDto, string? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a lead magnet permanently.
    /// </summary>
    /// <param name="id">The identifier of the lead magnet to delete.</param>
    /// <param name="userId">Optional user identifier. If provided, verifies ownership before deletion.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task DeleteAsync(int id, string? userId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes a lead magnet by setting IsActive to false.
    /// </summary>
    Task SoftDeleteAsync(int id, string? userId, CancellationToken cancellationToken = default);
}


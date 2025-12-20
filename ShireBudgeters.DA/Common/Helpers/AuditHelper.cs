using ShireBudgeters.DA.Common.Audit;

namespace ShireBudgeters.DA.Common.Helpers;

/// <summary>
/// Helper class for setting audit properties on entities.
/// </summary>
internal static class AuditHelper
{
    /// <summary>
    /// Sets the audit properties for a newly created entity.
    /// </summary>
    /// <param name="entity">The entity to set audit properties on.</param>
    /// <param name="userId">The identifier of the user creating the entity.</param>
    /// <exception cref="ArgumentNullException">Thrown when entity is null.</exception>
    public static void SetCreatedAudit(IAuditable entity, string? userId)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        entity.CreatedBy = userId;
        entity.CreatedDate = DateTime.UtcNow;
        entity.ModifiedBy = null;
        entity.ModifiedDate = null;
    }

    /// <summary>
    /// Sets the audit properties for an updated entity.
    /// </summary>
    /// <param name="entity">The entity to set audit properties on.</param>
    /// <param name="userId">The identifier of the user modifying the entity.</param>
    /// <exception cref="ArgumentNullException">Thrown when entity is null.</exception>
    public static void SetModifiedAudit(IAuditable entity, string? userId)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        entity.ModifiedBy = userId;
        entity.ModifiedDate = DateTime.UtcNow;

        // Preserve CreatedBy and CreatedDate if they haven't been set yet
        if (string.IsNullOrEmpty(entity.CreatedBy))
        {
            entity.CreatedBy = userId;
            entity.CreatedDate = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Sets audit properties based on whether this is a new or existing entity.
    /// </summary>
    /// <param name="entity">The entity to set audit properties on.</param>
    /// <param name="userId">The identifier of the user performing the operation.</param>
    /// <param name="isNew">True if this is a new entity, false if it's being updated.</param>
    public static void SetAudit(IAuditable entity, string? userId, bool isNew)
    {
        if (isNew)
        {
            SetCreatedAudit(entity, userId);
        }
        else
        {
            SetModifiedAudit(entity, userId);
        }
    }
}


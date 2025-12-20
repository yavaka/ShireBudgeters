namespace ShireBudgeters.DA.Models;

/// <summary>
/// Base class for models that support audit tracking.
/// Provides default implementation of IAuditable interface.
/// This base class is specific to the Data Access layer.
/// </summary>
public abstract class AuditableModel : IAuditable
{
    /// <summary>
    /// Gets or sets the identifier of the user who created the record.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified the record.
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }
}


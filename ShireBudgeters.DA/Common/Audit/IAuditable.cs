namespace ShireBudgeters.DA.Models;

/// <summary>
/// Interface for entities that support audit tracking (created/modified by and when).
/// This interface is specific to the Data Access layer.
/// </summary>
internal interface IAuditable
{
    /// <summary>
    /// Gets or sets the identifier of the user who created the record.
    /// </summary>
    string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was created.
    /// </summary>
    DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified the record.
    /// </summary>
    string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was last modified.
    /// </summary>
    DateTime? ModifiedDate { get; set; }
}


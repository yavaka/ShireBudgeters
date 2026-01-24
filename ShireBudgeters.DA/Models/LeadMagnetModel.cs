using ShireBudgeters.DA.Common.Audit;

namespace ShireBudgeters.DA.Models;

/// <summary>
/// Represents a lead magnet configuration that can be associated with a category.
/// </summary>
/// <remarks>A lead magnet stores configuration for lead capture forms, including form action URLs
/// and download file URLs. Each lead magnet is associated with a category and provides downloadable
/// assets to users who complete the lead capture form. Inherits audit properties from AuditableModel.</remarks>
public class LeadMagnetModel : AuditableModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the lead magnet.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the category associated with this lead magnet.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the display title for the lead magnet (e.g., "Free Budgeting Excel Template").
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the endpoint URL for the email marketing service where form submissions are sent.
    /// </summary>
    public string? FormActionUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL for the downloadable asset (PDF, Excel, etc.) provided after user signup.
    /// </summary>
    public string? DownloadFileUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the lead magnet is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation Properties

    /// <summary>
    /// Gets or sets the category associated with this lead magnet.
    /// </summary>
    public virtual CategoryModel? Category { get; set; }
}

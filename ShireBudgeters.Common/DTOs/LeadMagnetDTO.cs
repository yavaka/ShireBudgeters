namespace ShireBudgeters.Common.DTOs;

/// <summary>
/// Data Transfer Object for lead magnet information.
/// </summary>
/// <remarks>Represents a lead magnet configuration with form action URLs and download file URLs.
/// Includes all lead magnet properties and audit information for data transfer between layers.</remarks>
public class LeadMagnetDTO
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

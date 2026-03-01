namespace ShireBudgeters.Common.DTOs;

/// <summary>
/// Data Transfer Object for a single breadcrumb navigation item.
/// </summary>
/// <remarks>Used to build hierarchical trails (e.g. Home > Finance > Saving > Article Title).
/// When Url is null, the item represents the current page and is typically not rendered as a link.</remarks>
public class BreadcrumbItemDTO
{
    /// <summary>
    /// Gets or sets the display text for the breadcrumb item.
    /// </summary>
    public required string Text { get; set; }

    /// <summary>
    /// Gets or sets the URL for the breadcrumb link. Null for the current page (last item).
    /// </summary>
    public string? Url { get; set; }
}

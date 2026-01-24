namespace ShireBudgeters.Common.DTOs;

/// <summary>
/// Data Transfer Object for blog post information.
/// </summary>
/// <remarks>Represents a blog post with content, metadata, and publication information. Includes all post properties
/// and audit information for data transfer between layers. DTOs ensure type safety and decouple layers from internal model structures.</remarks>
public class PostDTO
{
    /// <summary>
    /// Gets or sets the unique identifier of the post.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who authored the post.
    /// </summary>
    public required string AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated category.
    /// </summary>
    public int? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the title of the blog post.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the URL-friendly version of the title, used for SEO-friendly URLs.
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    /// Gets or sets the full HTML/Markdown content of the post.
    /// </summary>
    public string? ContentBody { get; set; }

    /// <summary>
    /// Gets or sets the link to the static asset image file used as the featured image.
    /// </summary>
    public string? FeaturedImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the SEO description for search engines and social media previews.
    /// </summary>
    public string? MetaDescription { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the post was or will be published.
    /// </summary>
    public DateTime PublicationDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the post is published (true) or in draft state (false).
    /// </summary>
    public bool IsPublished { get; set; } = false;

    /// <summary>
    /// Gets or sets the identifier of the user who created the post.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the post was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified the post.
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the post was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }
}


using ShireBudgeters.DA.Common.Audit;

namespace ShireBudgeters.DA.Models;

/// <summary>
/// Represents a blog post that can be published and associated with a category and author.
/// </summary>
/// <remarks>A blog post contains content, metadata, and publication information. Each post is authored by a user
/// and may be associated with a category. Posts can be in draft or published state, with support for scheduled publishing.
/// Inherits audit properties from AuditableModel.</remarks>
public class PostModel : AuditableModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the post.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who authored this post.
    /// </summary>
    public required string AuthorId { get; set; }

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
    /// Gets or sets the date and time when the post was or will be published.
    /// </summary>
    public DateTime PublicationDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the post is published (true) or in draft state (false).
    /// </summary>
    public bool IsPublished { get; set; } = false;

    /// <summary>
    /// Gets or sets the identifier of the category associated with this post.
    /// </summary>
    public int? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the SEO description for search engines and social media previews.
    /// </summary>
    public string? MetaDescription { get; set; }

    // Navigation Properties

    /// <summary>
    /// Gets or sets the user who authored this post.
    /// </summary>
    public virtual UserModel? Author { get; set; }

    /// <summary>
    /// Gets or sets the category associated with this post.
    /// </summary>
    public virtual CategoryModel? Category { get; set; }
}


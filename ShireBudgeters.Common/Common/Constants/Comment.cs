namespace ShireBudgeters.Common.Common.Constants;

/// <summary>
/// Constants for comment-related validation and security.
/// </summary>
public static class Comment
{
    /// <summary>
    /// Maximum allowed length for author name in characters.
    /// </summary>
    public const int AuthorNameMaxLength = 100;

    /// <summary>
    /// Maximum allowed length for author email in characters.
    /// </summary>
    public const int AuthorEmailMaxLength = 255;

    /// <summary>
    /// Maximum allowed length for content body in characters.
    /// </summary>
    public const int ContentBodyMaxLength = 4000;

}

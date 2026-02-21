namespace ShireBudgeters.Configurations;

/// <summary>
/// Configuration for the public homepage latest content blocks. Not tied to any DB user.
/// </summary>
public class HomePageOptions
{
    /// <summary>
    /// Ordered list of blocks: section title and category ID. Category IDs are resolved without user context.
    /// </summary>
    public List<HomePageBlockOption> LatestBlocks { get; set; } = new();
}

/// <summary>
/// One thematic block on the homepage (e.g. "Latest in Personal Finance").
/// </summary>
public class HomePageBlockOption
{
    /// <summary>
    /// Display title for the section (e.g. "Latest in Personal Finance").
    /// </summary>
    public string SectionTitle { get; set; } = string.Empty;

    /// <summary>
    /// Category ID for this block. Only active categories are shown; zero or invalid IDs are skipped.
    /// </summary>
    public int CategoryId { get; set; }
}

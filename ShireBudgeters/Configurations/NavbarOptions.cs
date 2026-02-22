namespace ShireBudgeters.Configurations;

/// <summary>
/// Configuration for the public navbar category dropdowns.
/// Defines which categories appear in the nav and in what order (by slug).
/// </summary>
public class NavbarOptions
{
    /// <summary>
    /// Ordered list of root category slugs to display in the navbar (e.g. "finance", "tech").
    /// Only active categories with matching slugs are shown; missing slugs are skipped.
    /// </summary>
    public List<string> CategorySlugs { get; set; } = new();
}

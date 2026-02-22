namespace ShireBudgeters.Configurations;

/// <summary>
/// Configuration for the public footer: social media URLs and any future footer settings.
/// Only non-empty URLs are shown in the footer.
/// </summary>
public class FooterOptions
{
    /// <summary>Twitter/X profile URL.</summary>
    public string? Twitter { get; set; }

    /// <summary>Facebook page or profile URL.</summary>
    public string? Facebook { get; set; }

    /// <summary>Instagram profile URL.</summary>
    public string? Instagram { get; set; }

    /// <summary>LinkedIn profile or company URL.</summary>
    public string? LinkedIn { get; set; }

    /// <summary>YouTube channel or profile URL.</summary>
    public string? YouTube { get; set; }
}

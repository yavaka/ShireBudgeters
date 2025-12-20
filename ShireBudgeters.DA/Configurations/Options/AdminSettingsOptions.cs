namespace ShireBudgeters.DA.Configurations.Options;

/// <summary>
/// Options for the admin settings.
/// </summary>
/// <param name="Emails">The list of emails of the admins.</param>
internal record AdminSettingsOptions(List<string> Emails) 
{
    /// <summary>
    /// The section name of the admin settings.
    /// </summary>
    public const string SectionName = "AdminSettings";
};

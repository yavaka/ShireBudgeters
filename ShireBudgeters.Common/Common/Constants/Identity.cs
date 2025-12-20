using System.Runtime;

namespace ShireBudgeters.Common.Common.Constants;

/// <summary>
/// Constants for user roles in the application.
/// </summary>
public static class Identity
{
    public const string AdminRole = "Admin";
    public const string LoginPath = "/Identity/Login";
    public const string AccessDeniedPath = "/Identity/AccessDenied";
    public const string CookieName = "ShireBudgeters.Auth";    
}


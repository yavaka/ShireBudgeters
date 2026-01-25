using System.Text.RegularExpressions;

namespace ShireBudgeters.Components.Pages.Admin.Category.Components;

internal static partial class CategoryDialogColorHelper
{
    /// <summary>
    /// Normalizes color input into one of:
    /// - Hex format (if already hex, or parsed from rgb/rgba)
    /// - CSS color name (letters only)
    /// - null (if empty or invalid/unrecognized)
    /// </summary>
    internal static string? NormalizeColor(string? color)
    {
        if (string.IsNullOrWhiteSpace(color))
        {
            return null;
        }

        color = color.Trim();

        // If already in hex format, return as is.
        if (color.StartsWith('#'))
        {
            return color;
        }

        // If it's a CSS color name (letters only), return as is.
        if (LettersOnlyRegex().IsMatch(color))
        {
            return color;
        }

        // Try to parse RGB/RGBA format and convert to hex.
        var rgbMatch = RgbRegex().Match(color);
        if (rgbMatch.Success)
        {
            var r = int.Parse(rgbMatch.Groups[1].Value);
            var g = int.Parse(rgbMatch.Groups[2].Value);
            var b = int.Parse(rgbMatch.Groups[3].Value);
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        // If we can't convert it, return null (will be validated on submit).
        return null;
    }

    [GeneratedRegex(@"^[a-zA-Z]+$")]
    private static partial Regex LettersOnlyRegex();

    [GeneratedRegex(@"rgba?\((\d+),\s*(\d+),\s*(\d+)")]
    private static partial Regex RgbRegex();
}


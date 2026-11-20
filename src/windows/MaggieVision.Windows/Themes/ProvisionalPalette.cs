using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace MaggieVision_Windows.Themes;

/// <summary>Resolves the configurable shell palette for controls created in code.</summary>
internal static class ProvisionalPalette
{
    public static Brush GetBrush(string key, ElementTheme actualTheme)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        var themeKey = actualTheme == ElementTheme.Light ? "Light" : "Dark";

        foreach (var dictionary in Application.Current.Resources.MergedDictionaries)
        {
            if (dictionary.ThemeDictionaries.TryGetValue(themeKey, out var themeResources) &&
                themeResources is ResourceDictionary palette &&
                palette.TryGetValue(key, out var brush) &&
                brush is Brush typedBrush)
            {
                return typedBrush;
            }
        }

        throw new KeyNotFoundException($"The provisional {themeKey} theme has no brush named '{key}'.");
    }
}

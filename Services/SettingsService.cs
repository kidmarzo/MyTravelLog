namespace MyTravelLog.Services;

/// <summary>
/// Persists user preferences (dark mode, font size) using
/// .NET MAUI's Preferences API which writes to the platform's
/// native key-value store (SharedPreferences on Android,
/// NSUserDefaults on iOS, isolated storage on Windows).
/// </summary>
public class SettingsService
{
    private const string DarkModeKey   = "dark_mode";
    private const string FontSizeKey   = "font_size";
    private const double DefaultFontSize = 16.0;

    // ── Dark Mode ──────────────────────────────────────────────────────────

    /// <summary>Gets or sets whether dark mode is active.</summary>
    public bool IsDarkMode
    {
        get => Preferences.Default.Get(DarkModeKey, false);
        set => Preferences.Default.Set(DarkModeKey, value);
    }

    // ── Font Size ──────────────────────────────────────────────────────────

    /// <summary>Gets or sets the global font size in points.</summary>
    public double FontSize
    {
        get => Preferences.Default.Get(FontSizeKey, DefaultFontSize);
        set => Preferences.Default.Set(FontSizeKey, value);
    }

    // ── Reset ──────────────────────────────────────────────────────────────

    /// <summary>Resets all preferences to their factory defaults.</summary>
    public void ResetToDefaults()
    {
        IsDarkMode = false;
        FontSize   = DefaultFontSize;
    }
}

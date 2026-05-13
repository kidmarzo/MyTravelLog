using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyTravelLog.Services;

namespace MyTravelLog.ViewModels;

/// <summary>
/// ViewModel for the Settings page.
/// Manages theme switching, font size selection and preference persistence.
/// Changes are applied immediately and survive app restarts.
/// </summary>
public partial class SettingsViewModel : BaseViewModel
{
    private readonly SettingsService _settingsService;

    public SettingsViewModel(SettingsService settingsService)
    {
        _settingsService = settingsService;
        Title = "Settings";

        // Load persisted values on construction
        _isDarkMode  = _settingsService.IsDarkMode;
        _selectedFontSize = _settingsService.FontSize;
    }

    // ── Dark Mode ────────────────────────────────────────────────────────

    [ObservableProperty]
    private bool _isDarkMode;

    partial void OnIsDarkModeChanged(bool value)
    {
        _settingsService.IsDarkMode = value;

        // Apply theme change immediately via the App class
        if (Application.Current is App app)
            app.ApplyTheme(value);
    }

    // ── Font Size ────────────────────────────────────────────────────────

    /// <summary>Available font size options shown in the picker.</summary>
    public List<FontSizeOption> FontSizeOptions { get; } = new()
    {
        new FontSizeOption("Small",  14.0),
        new FontSizeOption("Medium", 18.0),
        new FontSizeOption("Large",  22.0),
    };

    [ObservableProperty]
    private double _selectedFontSize;

    partial void OnSelectedFontSizeChanged(double value)
    {
        _settingsService.FontSize = value;

        // Broadcast to allow any bound label to react
        if (Application.Current != null)
        {
            Application.Current.Resources["GlobalFontSize"] = value;
        }
    }

    /// <summary>Index into FontSizeOptions bound to the Picker control.</summary>
    public int SelectedFontIndex
    {
        get => FontSizeOptions.FindIndex(o => Math.Abs(o.Size - SelectedFontSize) < 0.1);
        set
        {
            if (value >= 0 && value < FontSizeOptions.Count)
                SelectedFontSize = FontSizeOptions[value].Size;
        }
    }

    // ── Reset ────────────────────────────────────────────────────────────

    [RelayCommand]
    private async Task ResetDefaults()
    {
        bool confirmed = await Shell.Current.DisplayAlert(
            "Reset Settings",
            "This will restore all settings to their defaults. Continue?",
            "Reset", "Cancel");

        if (!confirmed) return;

        _settingsService.ResetToDefaults();

        // Reload values
        IsDarkMode       = _settingsService.IsDarkMode;
        SelectedFontSize = _settingsService.FontSize;

        await Shell.Current.DisplayAlert("Done", "Settings have been reset.", "OK");
    }
}

/// <summary>Simple DTO representing a font size picker option.</summary>
public record FontSizeOption(string Label, double Size);

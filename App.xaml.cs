using MyTravelLog.Services;

namespace MyTravelLog;

public partial class App : Application
{
    private readonly SettingsService _settingsService;

    public App(SettingsService settingsService)
    {
        InitializeComponent();
        _settingsService = settingsService;
        ApplyTheme(_settingsService.IsDarkMode);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    public void ApplyTheme(bool isDark)
    {
        UserAppTheme = isDark ? AppTheme.Dark : AppTheme.Light;
    }
}
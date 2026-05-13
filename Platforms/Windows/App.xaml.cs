using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project files, see: http://aka.ms/winui-project-info.

namespace MyTravelLog.WinUI;

/// <summary>
/// Provides application-specific behaviour to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    /// <summary>
    /// Initialises the singleton application object.
    /// This is the first line of authored code executed,
    /// and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

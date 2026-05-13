namespace MyTravelLog;

/// <summary>
/// Code-behind for the application shell.
/// Registers named routes for pages that are navigated to
/// programmatically (not declared as tab items).
/// </summary>
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register detail pages so Shell.GoToAsync can navigate to them
        Routing.RegisterRoute(nameof(Views.AddPlacePage), typeof(Views.AddPlacePage));
        Routing.RegisterRoute(nameof(Views.PlaceDetailPage), typeof(Views.PlaceDetailPage));
    }
}

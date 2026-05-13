using Android.App;
using Android.Content.PM;

namespace MyTravelLog;

/// <summary>
/// Android main activity.
/// ConfigurationChanges settings prevent activity recreation on
/// screen rotation, which would otherwise reset navigation state.
/// </summary>
[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    ConfigurationChanges =
        ConfigChanges.ScreenSize   |
        ConfigChanges.Orientation  |
        ConfigChanges.UiMode       |
        ConfigChanges.ScreenLayout |
        ConfigChanges.SmallestScreenSize |
        ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}

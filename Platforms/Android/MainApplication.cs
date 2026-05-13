using Android.App;
using Android.Runtime;

namespace MyTravelLog;

/// <summary>
/// Android application entry point.
/// The MainLauncherActivity is provided by the MAUI framework automatically.
/// </summary>
[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

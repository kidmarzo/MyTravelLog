using Microsoft.Extensions.Logging;
using MyTravelLog.Services;
using MyTravelLog.ViewModels;
using MyTravelLog.Views;

namespace MyTravelLog;

/// <summary>
/// Entry point for the MAUI application.
/// Registers all services, view models, and views
/// using dependency injection.
/// </summary>
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ── Services ──────────────────────────────────────────────────────
        // DatabaseService must be registered before PlaceDataService
        // because PlaceDataService depends on it
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<PlaceDataService>();
        builder.Services.AddSingleton<CameraService>();
        builder.Services.AddSingleton<LocationService>();
        builder.Services.AddSingleton<TextToSpeechService>();
        builder.Services.AddSingleton<HapticService>();
        builder.Services.AddSingleton<AccelerometerService>();
        builder.Services.AddSingleton<SettingsService>();

        // ── ViewModels ────────────────────────────────────────────────────
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddSingleton<PlacesListViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddTransient<AddPlaceViewModel>();
        builder.Services.AddTransient<PlaceDetailViewModel>();

        // ── Views ─────────────────────────────────────────────────────────
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<PlacesListPage>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddTransient<AddPlacePage>();
        builder.Services.AddTransient<PlaceDetailPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyTravelLog.Models;
using MyTravelLog.Services;

namespace MyTravelLog.ViewModels;

/// <summary>
/// ViewModel for the Place Detail page.
/// Receives the place Id via Shell query parameter, loads the model,
/// and exposes Text-to-Speech and Share commands.
/// </summary>
[QueryProperty(nameof(PlaceId), "PlaceId")]
public partial class PlaceDetailViewModel : BaseViewModel
{
    private readonly PlaceDataService    _placeDataService;
    private readonly TextToSpeechService _ttsService;
    private readonly HapticService       _hapticService;

    public PlaceDetailViewModel(
        PlaceDataService    placeDataService,
        TextToSpeechService ttsService,
        HapticService       hapticService)
    {
        _placeDataService = placeDataService;
        _ttsService       = ttsService;
        _hapticService    = hapticService;

        Title = "Place Details";
    }

    // ── Query parameter ──────────────────────────────────────────────────

    /// <summary>
    /// Set by Shell navigation; triggers loading the correct place.
    /// </summary>
    [ObservableProperty]
    private string _placeId = string.Empty;

    partial void OnPlaceIdChanged(string value)
    {
        if (int.TryParse(value, out var id))
            CurrentPlace = _placeDataService.GetById(id);
    }

    // ── Bound data ───────────────────────────────────────────────────────

    [ObservableProperty] private PlaceModel? _currentPlace;

    [ObservableProperty] private bool _isSpeaking;

    // ── Text-to-Speech command ───────────────────────────────────────────

    /// <summary>
    /// Reads the place description aloud using the device TTS engine.
    /// Hardware feature: Text-to-Speech.
    /// </summary>
    [RelayCommand]
    private async Task ReadAloud()
    {
        if (CurrentPlace is null) return;

        if (IsSpeaking)
        {
            _ttsService.Stop();
            IsSpeaking = false;
            return;
        }

        IsSpeaking = true;
        try
        {
            string textToRead =
                $"Place: {CurrentPlace.Name}. " +
                $"Located at {CurrentPlace.Address}. " +
                $"Description: {CurrentPlace.Description}. " +
                $"Visited on {CurrentPlace.FormattedDate}.";

            await _ttsService.SpeakAsync(textToRead);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(
                "Speech Error",
                $"Could not read text: {ex.Message}",
                "OK");
        }
        finally
        {
            IsSpeaking = false;
        }
    }

    // ── Share command ────────────────────────────────────────────────────

    /// <summary>
    /// Shares place details including photo using the native share sheet.
    /// On Android, photos are shared via FileProvider for security.
    /// Triggers haptic feedback as confirmation.
    /// </summary>
    [RelayCommand]
    private async Task SharePlace()
    {
        if (CurrentPlace is null) return;

        try
        {
            _hapticService.Vibrate(150);

            // Build rich text content
            string shareText =
                $"📍 {CurrentPlace.Name}\n\n" +
                $"📅 Visited: {CurrentPlace.FormattedDate}\n" +
                $"📌 Location: {CurrentPlace.Address}\n" +
                $"🌐 Coordinates: {CurrentPlace.CoordinatesDisplay}\n\n" +
                $"📝 {CurrentPlace.Description}\n\n" +
                $"Shared from MyTravelLog 🌍";

            // Check if photo exists on device storage
            bool photoExists = CurrentPlace.HasPhoto &&
                               !string.IsNullOrEmpty(CurrentPlace.PhotoPath) &&
                               File.Exists(CurrentPlace.PhotoPath);

            if (photoExists)
            {
                try
                {
                    // Share the photo file using MAUI's Share API
                    // MAUI handles the FileProvider internally on Android
                    await Share.Default.RequestAsync(new ShareFileRequest
                    {
                        Title = $"📍 {CurrentPlace.Name} — MyTravelLog",
                        File = new ShareFile(CurrentPlace.PhotoPath ?? string.Empty, "image/jpeg")
                    });

                    // Ask if they also want to share the text description
                    bool alsoShareText = await Shell.Current.DisplayAlert(
                        "Share Description Too?",
                        "Would you like to share the place description as text?",
                        "Yes, Share Text", "No Thanks");

                    if (alsoShareText)
                    {
                        await Share.Default.RequestAsync(new ShareTextRequest
                        {
                            Title = $"📍 {CurrentPlace.Name}",
                            Text = shareText,
                            Subject = $"MyTravelLog — {CurrentPlace.Name}"
                        });
                    }
                }
                catch (Exception)
                {
                    // If photo sharing fails fall back to text only
                    await Share.Default.RequestAsync(new ShareTextRequest
                    {
                        Title = $"📍 {CurrentPlace.Name}",
                        Text = shareText,
                        Subject = $"MyTravelLog — {CurrentPlace.Name}"
                    });
                }
            }
            else
            {
                // No photo available — share text only
                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Title = $"📍 {CurrentPlace.Name}",
                    Text = shareText,
                    Subject = $"MyTravelLog — {CurrentPlace.Name}"
                });
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(
                "Share Error",
                $"Could not share this place: {ex.Message}",
                "OK");
        }
    }

    // ── Back navigation ──────────────────────────────────────────────────

    [RelayCommand]
    private async Task GoBack()
    {
        _ttsService.Stop();
        await Shell.Current.GoToAsync("..");
    }
}

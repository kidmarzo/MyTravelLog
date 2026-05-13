using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyTravelLog.Helpers;
using MyTravelLog.Models;
using MyTravelLog.Services;

namespace MyTravelLog.ViewModels;

/// <summary>
/// ViewModel for the Add Place page.
/// Orchestrates camera capture, GPS lookup, form validation and saving.
/// Hardware used: Camera, GPS/Geocoding, Haptic Feedback, Accelerometer (shake-to-clear).
/// </summary>
public partial class AddPlaceViewModel : BaseViewModel
{
    // ── Injected services ────────────────────────────────────────────────
    private readonly CameraService _cameraService;
    private readonly LocationService _locationService;
    private readonly HapticService _hapticService;
    private readonly PlaceDataService _placeDataService;
    private readonly AccelerometerService _accelerometerService;

    public AddPlaceViewModel(
        CameraService cameraService,
        LocationService locationService,
        HapticService hapticService,
        PlaceDataService placeDataService,
        AccelerometerService accelerometerService)
    {
        _cameraService = cameraService;
        _locationService = locationService;
        _hapticService = hapticService;
        _placeDataService = placeDataService;
        _accelerometerService = accelerometerService;

        Title = "Add New Place";

        // Shake-to-clear wired in code so ViewModel owns the subscription
        _accelerometerService.ShakeDetected += OnShakeDetected;
    }

    // ── Form fields ──────────────────────────────────────────────────────

    [ObservableProperty] private string _placeName = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private string _photoPath = string.Empty;
    [ObservableProperty] private string _address = string.Empty;
    [ObservableProperty] private double _latitude;
    [ObservableProperty] private double _longitude;
    [ObservableProperty] private bool _locationFetched = false;

    // ── Validation error messages (empty = no error) ─────────────────────

    [ObservableProperty] private string _placeNameError = string.Empty;
    [ObservableProperty] private string _descriptionError = string.Empty;

    // ── Derived display helpers ──────────────────────────────────────────

    public bool HasPhoto => !string.IsNullOrEmpty(PhotoPath);
    public string LatDisplay => LocationFetched ? $"{Latitude:F5}°" : "—";
    public string LonDisplay => LocationFetched ? $"{Longitude:F5}°" : "—";

    // ── Camera command ───────────────────────────────────────────────────

    /// <summary>
    /// Opens the camera, saves the photo and triggers haptic feedback.
    /// Hardware feature: Camera + Haptic Feedback.
    /// </summary>
    [RelayCommand]
    private async Task CapturePhoto()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var path = await _cameraService.CapturePhotoAsync();
            if (path is not null)
            {
                PhotoPath = path;
                OnPropertyChanged(nameof(HasPhoto));

                // Hardware feature: haptic feedback on successful capture
                _hapticService.Click();
            }
        }
        catch (PermissionException)
        {
            await Shell.Current.DisplayAlert(
                "Camera Permission Required",
                "Please enable camera access in your device Settings to capture photos.",
                "OK");
        }
        catch (NotSupportedException ex)
        {
            await Shell.Current.DisplayAlert("Not Supported", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(
                "Camera Error",
                $"An unexpected error occurred: {ex.Message}",
                "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // ── GPS command ──────────────────────────────────────────────────────

    /// <summary>
    /// Fetches the current GPS position and reverse-geocodes it to an address.
    /// Hardware feature: GPS / Geolocation.
    /// </summary>
    [RelayCommand]
    private async Task GetLocation()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var location = await _locationService.GetCurrentLocationAsync();
            if (location is null)
            {
                await Shell.Current.DisplayAlert(
                    "Location Unavailable",
                    "Could not determine your current location. " +
                    "Please ensure Location Services are enabled and try again.",
                    "OK");
                return;
            }

            Latitude = location.Latitude;
            Longitude = location.Longitude;

            // Reverse geocode to get a human-readable address
            Address = await _locationService.ReverseGeocodeAsync(Latitude, Longitude);

            LocationFetched = true;
            OnPropertyChanged(nameof(LatDisplay));
            OnPropertyChanged(nameof(LonDisplay));
        }
        catch (PermissionException)
        {
            await Shell.Current.DisplayAlert(
                "Location Permission Required",
                "Please enable location access in your device Settings.",
                "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(
                "Location Error",
                $"Could not fetch location: {ex.Message}",
                "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // ── Save command ─────────────────────────────────────────────────────

    /// <summary>
    /// Validates all inputs and saves the new place to the data store.
    /// </summary>
    [RelayCommand]
    private async Task SavePlace()
    {
        if (IsBusy) return;

        // Run validation
        PlaceNameError = ValidationHelper.ValidatePlaceName(PlaceName);
        DescriptionError = ValidationHelper.ValidateDescription(Description);

        if (!ValidationHelper.IsValid(PlaceNameError, DescriptionError))
            return; // UI error labels are now visible via binding

        if (!LocationFetched)
        {
            await Shell.Current.DisplayAlert(
                "Location Required",
                "Please fetch your current location before saving.",
                "OK");
            return;
        }

        // Warn (not block) if no photo attached
        if (!HasPhoto)
        {
            bool proceed = await Shell.Current.DisplayAlert(
                "No Photo",
                "You haven't added a photo. Save without one?",
                "Save Anyway", "Go Back");
            if (!proceed) return;
        }

        IsBusy = true;
        try
        {
            var place = new PlaceModel
            {
                Name = PlaceName.Trim(),
                Description = Description.Trim(),
                PhotoPath = PhotoPath,
                Latitude = Latitude,
                Longitude = Longitude,
                Address = Address,
                DateAdded = DateTime.UtcNow
            };

            await _placeDataService.AddPlaceAsync(place);
            _hapticService.Click(); // Confirm save with haptic

            await Shell.Current.DisplayAlert("Saved!", $"'{place.Name}' has been logged.", "OK");
            ResetForm();
            await Shell.Current.GoToAsync("//PlacesListPage");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(
                "Save Error",
                $"Could not save place: {ex.Message}",
                "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // ── Clear / shake ────────────────────────────────────────────────────

    /// <summary>Clears all form fields and resets validation state.</summary>
    [RelayCommand]
    private async Task ClearForm()
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Clear Form",
            "Are you sure you want to clear all fields?",
            "Clear", "Cancel");

        if (!confirm) return;
        ResetForm();
    }

    /// <summary>Resets all form fields to their default empty state.</summary>
    private void ResetForm()
    {
        PlaceName = string.Empty;
        Description = string.Empty;
        PhotoPath = string.Empty;
        Address = string.Empty;
        Latitude = 0;
        Longitude = 0;
        LocationFetched = false;
        PlaceNameError = string.Empty;
        DescriptionError = string.Empty;
        OnPropertyChanged(nameof(HasPhoto));
        OnPropertyChanged(nameof(LatDisplay));
        OnPropertyChanged(nameof(LonDisplay));
    }

    /// <summary>
    /// Handles the shake gesture from AccelerometerService.
    /// Hardware feature #5: Accelerometer / shake-to-clear.
    /// </summary>
    private async void OnShakeDetected(object? sender, EventArgs e)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Shake Detected!",
            "Do you want to clear the form?",
            "Clear", "Cancel");

        if (confirm) ResetForm();
    }

    // ── Lifecycle ────────────────────────────────────────────────────────

    /// <summary>Start accelerometer when page appears.</summary>
    public void OnAppearing() => _accelerometerService.Start();

    /// <summary>Stop accelerometer when page disappears to save battery.</summary>
    public void OnDisappearing()
    {
        _accelerometerService.Stop();
        _accelerometerService.ShakeDetected -= OnShakeDetected;
    }
}
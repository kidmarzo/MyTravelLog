namespace MyTravelLog.Services;

/// <summary>
/// Wraps .NET MAUI's Geolocation and Geocoding APIs.
/// Provides the current GPS position and converts coordinates to a
/// human-readable address via reverse geocoding.
/// </summary>
public class LocationService
{
    /// <summary>
    /// Retrieves the device's current GPS coordinates.
    /// </summary>
    /// <returns>
    /// A <see cref="Location"/> object with Latitude and Longitude,
    /// or null when location is unavailable.
    /// </returns>
    /// <exception cref="PermissionException">
    /// Thrown when location permission is permanently denied.
    /// </exception>
    public async Task<Location?> GetCurrentLocationAsync()
    {
        // Request fine location permission
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        if (status == PermissionStatus.Denied)
            throw new PermissionException("Location permission denied.");

        var request = new GeolocationRequest(GeolocationAccuracy.Medium,
                                              TimeSpan.FromSeconds(15));

        return await Geolocation.Default.GetLocationAsync(request);
    }

    /// <summary>
    /// Converts GPS coordinates to a readable address string.
    /// Falls back to a coordinate string when geocoding fails.
    /// </summary>
    public async Task<string> ReverseGeocodeAsync(double latitude, double longitude)
    {
        try
        {
            var placemarks = await Geocoding.Default
                .GetPlacemarksAsync(latitude, longitude);

            var placemark = placemarks?.FirstOrDefault();
            if (placemark is null)
                return $"{latitude:F4}, {longitude:F4}";

            // Build a readable address from available placemark fields
            var parts = new[]
            {
                placemark.SubThoroughfare,
                placemark.Thoroughfare,
                placemark.Locality,
                placemark.AdminArea,
                placemark.CountryName
            }
            .Where(p => !string.IsNullOrWhiteSpace(p));

            return string.Join(", ", parts);
        }
        catch
        {
            // Geocoding can fail on emulators without network – fallback gracefully
            return $"{latitude:F4}°N, {longitude:F4}°E";
        }
    }
}

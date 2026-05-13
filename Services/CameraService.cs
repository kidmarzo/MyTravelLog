namespace MyTravelLog.Services;

/// <summary>
/// Wraps .NET MAUI's MediaPicker to capture photos with the device camera.
/// Saves captured images to the app's cache directory so they persist
/// across navigation but are cleaned up by the OS when storage is low.
/// </summary>
public class CameraService
{
    /// <summary>
    /// Opens the device camera, lets the user capture a photo, copies the
    /// result to the app cache and returns the local file path.
    /// </summary>
    /// <returns>
    /// The full path to the saved image file, or null if the user cancelled
    /// or permission was denied.
    /// </returns>
    /// <exception cref="PermissionException">
    /// Thrown when camera permission is permanently denied.
    /// The caller should show an alert directing the user to app Settings.
    /// </exception>
    public async Task<string?> CapturePhotoAsync()
    {
        // Check and request camera permission
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<Permissions.Camera>();

        if (status == PermissionStatus.Denied)
            throw new PermissionException("Camera permission denied.");

        // MediaPicker is not available on all platforms (e.g. some Windows configs)
        if (!MediaPicker.Default.IsCaptureSupported)
            throw new NotSupportedException("Camera capture is not supported on this device.");

        // Open native camera
        var photo = await MediaPicker.Default.CapturePhotoAsync();
        if (photo is null)
            return null; // User cancelled

        // Copy to cache so the temp URI remains valid after the picker closes
        var localPath = Path.Combine(FileSystem.CacheDirectory,
                                     $"place_{Guid.NewGuid()}.jpg");

        await using var sourceStream = await photo.OpenReadAsync();
        await using var destStream   = File.OpenWrite(localPath);
        await sourceStream.CopyToAsync(destStream);

        return localPath;
    }
}

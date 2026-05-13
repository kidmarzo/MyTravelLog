namespace MyTravelLog.Services;

/// <summary>
/// Wraps .NET MAUI's Accelerometer API.
/// Detects a shake gesture which can be used to clear the add-place form,
/// giving the user a quick reset shortcut.
///
/// Hardware feature #5 — satisfies the requirement for 4+ hardware features.
/// </summary>
public class AccelerometerService : IDisposable
{
    // Threshold G-force to recognise a shake (tweak if too sensitive)
    private const double ShakeThreshold = 2.5;

    /// <summary>
    /// Raised on the UI thread when a shake gesture is detected.
    /// Subscribe in the ViewModel or View that needs to respond.
    /// </summary>
    public event EventHandler? ShakeDetected;

    private bool _isRunning;

    /// <summary>
    /// Starts listening for accelerometer readings.
    /// Safe to call multiple times – redundant calls are ignored.
    /// </summary>
    public void Start()
    {
        if (_isRunning || !Accelerometer.Default.IsSupported)
            return;

        Accelerometer.Default.ReadingChanged += OnReadingChanged;
        Accelerometer.Default.Start(SensorSpeed.Game);
        _isRunning = true;
    }

    /// <summary>
    /// Stops the accelerometer listener to preserve battery.
    /// Call from the page's OnDisappearing override.
    /// </summary>
    public void Stop()
    {
        if (!_isRunning)
            return;

        Accelerometer.Default.Stop();
        Accelerometer.Default.ReadingChanged -= OnReadingChanged;
        _isRunning = false;
    }

    private void OnReadingChanged(object? sender, AccelerometerChangedEventArgs e)
    {
        var data = e.Reading;

        // Calculate resultant G-force across all three axes
        double magnitude = Math.Sqrt(
            data.Acceleration.X * data.Acceleration.X +
            data.Acceleration.Y * data.Acceleration.Y +
            data.Acceleration.Z * data.Acceleration.Z);

        if (magnitude > ShakeThreshold)
        {
            // Marshal back to the UI thread before raising the event
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var handler = ShakeDetected;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            });
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Stop();
        GC.SuppressFinalize(this);
    }
}

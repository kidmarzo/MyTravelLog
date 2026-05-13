namespace MyTravelLog.Services;

/// <summary>
/// Wraps .NET MAUI's HapticFeedback and Vibration APIs.
/// Provides short tactile confirmations for user actions.
/// </summary>
public class HapticService
{
    /// <summary>
    /// Triggers a short haptic click feedback.
    /// Used to confirm successful actions (e.g. photo captured).
    /// Silently ignored on platforms that do not support haptics.
    /// </summary>
    public void Click()
    {
        try
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
        }
        catch (FeatureNotSupportedException)
        {
            // Haptic feedback not available on this device/platform – ignore
        }
    }

    /// <summary>
    /// Triggers a longer vibration pattern.
    /// Used for secondary confirmations such as the Share button.
    /// </summary>
    public void Vibrate(int milliseconds = 200)
    {
        try
        {
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(milliseconds));
        }
        catch (FeatureNotSupportedException)
        {
            // Vibration not available on this platform – ignore
        }
    }
}

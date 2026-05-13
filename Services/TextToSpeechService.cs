namespace MyTravelLog.Services;

/// <summary>
/// Wraps .NET MAUI's TextToSpeech API.
/// Reads a given text string aloud using the device's built-in TTS engine.
/// </summary>
public class TextToSpeechService
{
    private CancellationTokenSource? _cts;

    /// <summary>
    /// Speaks the provided text.
    /// If TTS is already speaking, the previous utterance is cancelled first.
    /// </summary>
    /// <param name="text">The text to read aloud.</param>
    public async Task SpeakAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return;

        // Cancel any in-progress speech
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        var settings = new SpeechOptions
        {
            Volume = 1.0f,  // 0.0 – 1.0
            Pitch  = 1.0f   // 0.5 – 2.0 (1.0 = normal)
        };

        await TextToSpeech.Default.SpeakAsync(text, settings, _cts.Token);
    }

    /// <summary>
    /// Stops any currently active speech immediately.
    /// </summary>
    public void Stop()
    {
        _cts?.Cancel();
        _cts = null;
    }
}

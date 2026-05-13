using System.Globalization;

namespace MyTravelLog.Helpers;

// ════════════════════════════════════════════════════════════════
//  All value converters used in XAML bindings.
//  Registered as application-level resources in App.xaml.
// ════════════════════════════════════════════════════════════════

/// <summary>
/// Returns true when a string is non-null and non-empty.
/// Used to control error-label visibility.
/// </summary>
public class StringToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => !string.IsNullOrEmpty(value as string);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

/// <summary>
/// Inverts a boolean value.
/// Used to show placeholders when HasPhoto is false.
/// </summary>
public class InverseBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && !b;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && !b;
}

/// <summary>
/// Returns true when the value is null.
/// Used to show the "Place not found" label on the detail page.
/// </summary>
public class NullToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is null;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

/// <summary>
/// Converts the IsSpeaking bool to a speaker icon string.
/// Used on the Read Aloud button to toggle between play/stop icons.
/// </summary>
public class BoolToSpeakIconConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is true ? "⏹" : "🔊";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

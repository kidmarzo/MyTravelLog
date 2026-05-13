namespace MyTravelLog.Helpers;

/// <summary>
/// Stateless helper providing common input-validation rules.
/// Each method returns a user-friendly error message, or an empty string
/// when the value passes validation.
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Validates the place name field.
    /// Rules: not null/empty, at least 2 non-whitespace characters.
    /// </summary>
    public static string ValidatePlaceName(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "Place name is required.";

        if (value.Trim().Length < 2)
            return "Place name must be at least 2 characters.";

        return string.Empty;
    }

    /// <summary>
    /// Validates the description field.
    /// Rules: not null/empty, at least 10 non-whitespace characters.
    /// </summary>
    public static string ValidateDescription(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "Description is required.";

        if (value.Trim().Length < 10)
            return "Description must be at least 10 characters.";

        return string.Empty;
    }

    /// <summary>
    /// Returns true when no validation errors are present in the provided list.
    /// </summary>
    public static bool IsValid(params string[] errors)
        => errors.All(e => string.IsNullOrEmpty(e));
}

using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

// Add this alias to resolve the conflict
using TableAttribute = SQLite.TableAttribute;

namespace MyTravelLog.Models;

/// <summary>
/// Represents a single travel place entry logged by the user.
/// SQLite attributes map this class to a database table automatically.
/// </summary>
[SQLite.Table("Places")]
public class PlaceModel
{
    /// <summary>Primary key — auto incremented by SQLite.</summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>User-supplied name for the place.</summary>
    [NotNull]
    public string Name { get; set; } = string.Empty;

    /// <summary>User's notes about the visit.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Full file-system path to the captured photo.</summary>
    public string? PhotoPath { get; set; }

    /// <summary>GPS latitude in decimal degrees.</summary>
    public double Latitude { get; set; }

    /// <summary>GPS longitude in decimal degrees.</summary>
    public double Longitude { get; set; }

    /// <summary>Human-readable address from reverse geocoding.</summary>
    public string Address { get; set; } = "Address not available";

    /// <summary>UTC timestamp when the entry was created.</summary>
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    /// <summary>Formatted date string used in the UI.</summary>
    [Ignore]
    public string FormattedDate =>
        DateAdded.ToLocalTime().ToString("dd MMM yyyy, HH:mm");

    /// <summary>Truncated address for list views.</summary>
    [Ignore]
    public string ShortAddress =>
        Address.Length > 50 ? Address[..47] + "…" : Address;

    /// <summary>True when a photo has been captured.</summary>
    [Ignore]
    public bool HasPhoto => !string.IsNullOrEmpty(PhotoPath);

    /// <summary>Formatted coordinate string for display.</summary>
    [Ignore]
    public string CoordinatesDisplay =>
        $"{Latitude:F5}°, {Longitude:F5}°";
}

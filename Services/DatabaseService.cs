using SQLite;
using MyTravelLog.Models;

namespace MyTravelLog.Services;

/// <summary>
/// Manages all SQLite database operations for the app.
/// The database file is stored in the app's local data directory
/// which persists across app restarts and device reboots.
/// </summary>
public class DatabaseService
{
    private SQLiteAsyncConnection? _database;

    // Database file stored in app's private local folder
    private static readonly string DbPath =
        Path.Combine(FileSystem.AppDataDirectory, "mytravellog.db3");

    /// <summary>
    /// Initialises the database connection and creates the Places table
    /// if it does not already exist. Safe to call multiple times.
    /// </summary>
    private async Task InitialiseAsync()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(DbPath,
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create    |
            SQLiteOpenFlags.SharedCache);

        // Creates the table only if it doesn't already exist
        await _database.CreateTableAsync<PlaceModel>();
    }

    // ── READ ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns all saved places ordered by newest first.
    /// </summary>
    public async Task<List<PlaceModel>> GetAllPlacesAsync()
    {
        await InitialiseAsync();
        return await _database!
            .Table<PlaceModel>()
            .OrderByDescending(p => p.DateAdded)
            .ToListAsync();
    }

    /// <summary>
    /// Returns a single place by its database ID.
    /// Returns null when not found.
    /// </summary>
    public async Task<PlaceModel?> GetPlaceByIdAsync(int id)
    {
        await InitialiseAsync();
        return await _database!
            .Table<PlaceModel>()
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    // ── WRITE ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Inserts a new place into the database.
    /// Returns the number of rows inserted (should be 1).
    /// </summary>
    public async Task<int> AddPlaceAsync(PlaceModel place)
    {
        await InitialiseAsync();
        return await _database!.InsertAsync(place);
    }

    /// <summary>
    /// Updates an existing place record in the database.
    /// </summary>
    public async Task<int> UpdatePlaceAsync(PlaceModel place)
    {
        await InitialiseAsync();
        return await _database!.UpdateAsync(place);
    }

    // ── DELETE ────────────────────────────────────────────────────────────

    /// <summary>
    /// Deletes a place from the database by reference.
    /// Also deletes the associated photo file from cache.
    /// </summary>
    public async Task<int> DeletePlaceAsync(PlaceModel place)
    {
        await InitialiseAsync();

        // Clean up photo file from device storage
        if (!string.IsNullOrEmpty(place.PhotoPath) &&
            File.Exists(place.PhotoPath))
        {
            try { File.Delete(place.PhotoPath); }
            catch { /* ignore file deletion errors */ }
        }

        return await _database!.DeleteAsync(place);
    }
}

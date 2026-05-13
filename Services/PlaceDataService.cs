using System.Collections.ObjectModel;
using MyTravelLog.Models;

namespace MyTravelLog.Services;

/// <summary>
/// Bridges the DatabaseService (SQLite persistence) and the UI layer
/// (ObservableCollection for live binding).
/// Loads from the database on first access and keeps the in-memory
/// collection in sync with every write operation.
/// </summary>
public class PlaceDataService
{
    private readonly DatabaseService _databaseService;
    private bool _isLoaded = false;

    public PlaceDataService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    /// <summary>
    /// Live collection bound to CollectionViews throughout the app.
    /// Automatically updates the UI when items are added or removed.
    /// </summary>
    public ObservableCollection<PlaceModel> Places { get; } = new();

    /// <summary>
    /// Loads all places from the database into the observable collection.
    /// Only hits the database once per app session.
    /// </summary>
    public async Task LoadPlacesAsync()
    {
        if (_isLoaded) return;

        var places = await _databaseService.GetAllPlacesAsync();
        Places.Clear();
        foreach (var place in places)
            Places.Add(place);

        _isLoaded = true;
    }

    /// <summary>
    /// Adds a new place to both the database and the observable collection.
    /// </summary>
    public async Task AddPlaceAsync(PlaceModel place)
    {
        ArgumentNullException.ThrowIfNull(place);
        await _databaseService.AddPlaceAsync(place);
        Places.Insert(0, place); // Newest first
    }

    /// <summary>
    /// Removes a place from both the database and the observable collection.
    /// </summary>
    public async Task RemovePlaceAsync(PlaceModel place)
    {
        ArgumentNullException.ThrowIfNull(place);
        await _databaseService.DeletePlaceAsync(place);
        Places.Remove(place);
    }

    /// <summary>
    /// Finds a place by its database ID.
    /// Returns null when not found.
    /// </summary>
    public PlaceModel? GetById(int id)
        => Places.FirstOrDefault(p => p.Id == id);
}
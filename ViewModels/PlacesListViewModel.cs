using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyTravelLog.Models;
using MyTravelLog.Services;

namespace MyTravelLog.ViewModels;

/// <summary>
/// ViewModel for the Places List page.
/// Loads places from SQLite on first appearance and keeps
/// the UI in sync with add/delete operations.
/// </summary>
public partial class PlacesListViewModel : BaseViewModel
{
    private readonly PlaceDataService _placeDataService;

    public PlacesListViewModel(PlaceDataService placeDataService)
    {
        _placeDataService = placeDataService;
        Title = "My Places";
    }

    /// <summary>Bound to the CollectionView ItemsSource.</summary>
    public ObservableCollection<PlaceModel> Places => _placeDataService.Places;

    /// <summary>True when collection is empty — shows empty state.</summary>
    public bool IsEmpty => Places.Count == 0;

    [ObservableProperty] private PlaceModel? _selectedPlace;

    // ── Load data ─────────────────────────────────────────────────────────

    /// <summary>
    /// Called from OnAppearing — loads places from SQLite database.
    /// </summary>
    public async Task OnAppearing()
    {
        await _placeDataService.LoadPlacesAsync();
        OnPropertyChanged(nameof(IsEmpty));
    }

    // ── Navigation ────────────────────────────────────────────────────────

    [RelayCommand]
    private async Task SelectPlace(PlaceModel place)
    {
        if (place is null) return;
        var parameters = new Dictionary<string, object>
        {
            ["PlaceId"] = place.Id.ToString()
        };
        await Shell.Current.GoToAsync(
            nameof(Views.PlaceDetailPage), parameters);
    }

    [RelayCommand]
    private async Task AddNewPlace()
    {
        await Shell.Current.GoToAsync(nameof(Views.AddPlacePage));
    }

    // ── Delete ────────────────────────────────────────────────────────────

    [RelayCommand]
    private async Task DeletePlace(PlaceModel place)
    {
        if (place is null) return;

        bool confirmed = await Shell.Current.DisplayAlert(
            "Delete Place",
            $"Are you sure you want to delete '{place.Name}'?",
            "Delete", "Cancel");

        if (!confirmed) return;

        try
        {
            await _placeDataService.RemovePlaceAsync(place);
            OnPropertyChanged(nameof(IsEmpty));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(
                "Delete Error",
                $"Could not delete place: {ex.Message}",
                "OK");
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MyTravelLog.ViewModels;

/// <summary>
/// ViewModel for the Home (landing) page.
/// Exposes navigation commands for each of the three action cards.
/// </summary>
public partial class HomeViewModel : BaseViewModel
{
    public HomeViewModel()
    {
        Title = "MyTravelLog";
    }

    /// <summary>Navigates to the Add New Place form.</summary>
    [RelayCommand]
    private async Task GoToAddPlace()
    {
        await Shell.Current.GoToAsync(nameof(Views.AddPlacePage));
    }

    /// <summary>Navigates to the Places list tab.</summary>
    [RelayCommand]
    private async Task GoToPlacesList()
    {
        await Shell.Current.GoToAsync("//PlacesListPage");
    }

    /// <summary>Navigates to the Settings tab.</summary>
    [RelayCommand]
    private async Task GoToSettings()
    {
        await Shell.Current.GoToAsync("//SettingsPage");
    }
}

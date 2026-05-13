using MyTravelLog.ViewModels;

namespace MyTravelLog.Views;

/// <summary>
/// Code-behind for HomePage.
/// Receives the ViewModel via dependency injection and sets the BindingContext.
/// All logic lives in HomeViewModel.
/// </summary>
public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

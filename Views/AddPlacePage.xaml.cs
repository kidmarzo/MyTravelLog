using MyTravelLog.ViewModels;

namespace MyTravelLog.Views;

/// <summary>
/// Code-behind for AddPlacePage.
/// Notifies the ViewModel of page lifecycle events so the accelerometer
/// can be started and stopped at the right time.
/// </summary>
public partial class AddPlacePage : ContentPage
{
    private readonly AddPlaceViewModel _viewModel;

    public AddPlacePage(AddPlaceViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    /// <summary>
    /// Register a converter for error-label visibility in code behind
    /// since converters must be in ResourceDictionary or App.Resources.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing(); // Start accelerometer
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.OnDisappearing(); // Stop accelerometer
    }
}

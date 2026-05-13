using MyTravelLog.ViewModels;

namespace MyTravelLog.Views;

/// <summary>
/// Code-behind for PlaceDetailPage.
/// Stops TTS if the user navigates away mid-speech.
/// </summary>
public partial class PlaceDetailPage : ContentPage
{
    private readonly PlaceDetailViewModel _viewModel;

    public PlaceDetailPage(PlaceDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Ensure TTS is stopped when leaving the page
        _viewModel.GoBackCommand.Execute(null);
    }
}

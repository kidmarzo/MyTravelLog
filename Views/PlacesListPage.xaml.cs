using MyTravelLog.ViewModels;

namespace MyTravelLog.Views;

public partial class PlacesListPage : ContentPage
{
    private readonly PlacesListViewModel _viewModel;

    public PlacesListPage(PlacesListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearing();
    }
}
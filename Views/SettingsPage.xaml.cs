using MyTravelLog.ViewModels;

namespace MyTravelLog.Views;

/// <summary>
/// Code-behind for SettingsPage.
/// All logic lives in SettingsViewModel.
/// </summary>
public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

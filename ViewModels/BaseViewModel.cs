using CommunityToolkit.Mvvm.ComponentModel;

namespace MyTravelLog.ViewModels;

/// <summary>
/// Base class for all ViewModels.
/// Inherits ObservableObject from CommunityToolkit.Mvvm which provides
/// INotifyPropertyChanged without boilerplate.
///
/// Exposes two universal properties used by every page:
///   • IsBusy  – drives loading indicators
///   • Title   – bound to the page's navigation bar title
/// </summary>
public partial class BaseViewModel : ObservableObject
{
    /// <summary>
    /// True while an async operation is in progress.
    /// Bind to an ActivityIndicator's IsRunning and IsVisible properties.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    /// <summary>Inverse of IsBusy – convenient for button IsEnabled bindings.</summary>
    public bool IsNotBusy => !IsBusy;

    /// <summary>Navigation bar title for the page.</summary>
    [ObservableProperty]
    private string _title = string.Empty;
}

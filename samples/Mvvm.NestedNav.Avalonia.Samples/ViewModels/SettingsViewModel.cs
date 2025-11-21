using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mvvm.NestedNav.Avalonia.Samples.Screens;

namespace Mvvm.NestedNav.Avalonia.Samples.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty] private string _greeting = "Welcome to settings page!";

    public override Task LoadAsync(INavigator navigator, Screen screen, CancellationToken cancellationToken = default)
    {
        return base.LoadAsync(navigator, screen, cancellationToken);
    }

    [RelayCommand]
    private void GoToDetails()
    {
        Navigator.Navigate(new DetailsScreen("Passed from Settings!"));
    }
}
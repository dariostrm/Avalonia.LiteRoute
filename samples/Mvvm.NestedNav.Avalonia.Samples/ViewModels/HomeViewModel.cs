using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mvvm.NestedNav.Avalonia.Samples.Screens;

namespace Mvvm.NestedNav.Avalonia.Samples.ViewModels;

[ObservableObject]
public partial class HomeViewModel : ScreenViewModel
{
    [ObservableProperty] private string _greeting = "Home not yet loaded!";

    public override void Initialize(INavigator navigator, Screen screen)
    {
        base.Initialize(navigator, screen);
        Greeting = "Welcome to home page!";
    }

    [RelayCommand]
    private void GoToDetails()
    {
        Navigator.Navigate(new DetailsScreen("Passed from Home!"));
    }
}
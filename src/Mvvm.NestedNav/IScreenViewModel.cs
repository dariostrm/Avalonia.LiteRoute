using System.ComponentModel;

namespace Mvvm.NestedNav;

public interface IScreenViewModel
{
    void Initialize(INavigator navigator, Screen screen);
    
    void OnNavigatedTo();
    void OnNavigatingFrom();
    void OnNavigatedFrom();
    void OnClosing();
}
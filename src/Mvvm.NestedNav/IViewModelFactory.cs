using Mvvm.NestedNav;

namespace Mvvm.NestedNav;

public interface IViewModelFactory
{
    IScreenViewModel ResolveViewModel<TScreen>() 
        where TScreen : Screen;
    
    IScreenViewModel CreateViewModel(Screen screen, INavigator navigator);
}
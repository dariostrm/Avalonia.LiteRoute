using Mvvm.NestedNav;

namespace Mvvm.NestedNav;

public interface IViewModelResolver
{
    IScreenViewModel ResolveViewModel<TScreen>() 
        where TScreen : Screen;
    
    IScreenViewModel ResolveViewModel(Screen screen);
}
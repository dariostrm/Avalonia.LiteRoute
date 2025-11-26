namespace Mvvm.NestedNav;

public record NavEntry(Screen Screen, IScreenViewModel ViewModel)
{
    public static NavEntry Create(Screen screen, IViewModelResolver viewModelResolver)
    {
        return new NavEntry(screen, viewModelResolver.ResolveViewModel(screen));
    }
}
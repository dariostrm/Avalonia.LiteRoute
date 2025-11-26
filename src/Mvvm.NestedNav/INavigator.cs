using System.Collections.Immutable;

namespace Mvvm.NestedNav;

public interface INavigator : IDisposable
{
    // Navigation Stack
    IObservable<NavBackStack> BackStack { get; }
    NavBackStack BackStackValue { get; }
    
    INavigator? ParentNavigator { get; }
    bool CanGoBack { get; }
    
    // Navigation Methods
    void OverrideBackStack(IEnumerable<Screen> screens);
    void Navigate(Screen screen);
    void GoBack();
    void GoBackOrClear();
    void GoBackTo(Screen screen);
    void ClearAndSet(Screen screen);
    void Clear();
    void ReplaceCurrent(Screen screen);
    
    //Hooks
    IObservable<NavigatingEventArgs> Navigating { get; }
    IObservable<NavigatedEventArgs> Navigated { get; }
}

public record NavigatingEventArgs(Screen? OldScreen, IScreenViewModel? OldViewModel, Screen? NewScreen);
public record NavigatedEventArgs(Screen? OldScreen, Screen? NewScreen, IScreenViewModel? NewViewModel);
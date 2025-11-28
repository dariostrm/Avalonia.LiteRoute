using System.Collections.Immutable;

namespace Mvvm.NestedNav;

public interface INavigator
{
    IImmutableStack<NavEntry> BackStack { get; }
    
    INavigator? ParentNavigator { get; }
    
    // Navigation Methods
    bool CanGoBack();
    void OverrideBackStack(IEnumerable<Route> routes);
    void Navigate(Route route);
    void GoBack();
    void GoBackTo(Route route);
    void ClearAndSet(Route route);
    void ReplaceCurrent(Route route);
    
    event EventHandler<NavigatingEventArgs>? Navigating;
    event EventHandler<NavigatedEventArgs>? Navigated;
}

public record NavigatingEventArgs(Route OldRoute, IViewModel OldViewModel, Route NewRoute);
public record NavigatedEventArgs(Route OldRoute, Route NewRoute, IViewModel NewViewModel);
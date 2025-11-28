
using System.Collections.Immutable;

namespace Mvvm.NestedNav;

public class Navigator : INavigator
{
    private readonly IViewModelFactory _viewModelFactory;
    
    public IImmutableStack<NavEntry> BackStack { get; private set; }
    public INavigator? ParentNavigator { get; }
    
    public event EventHandler<NavigatingEventArgs>? Navigating;
    public event EventHandler<NavigatedEventArgs>? Navigated;
    
    public Navigator(IViewModelFactory viewModelFactory, Route initialRoute, INavigator? parentNavigator = null)
    {
        _viewModelFactory = viewModelFactory;
        ParentNavigator = parentNavigator;
        var initialEntry = CreateEntry(initialRoute);
        BackStack = ImmutableStack.Create(initialEntry);
    }

    public virtual bool CanGoBack() => BackStack.Count() > 1;

    public void OverrideBackStack(IEnumerable<Route> routes)
    {
        var newBackStack = routes
            .Select(CreateEntry)
            .Aggregate(ImmutableStack<NavEntry>.Empty, (backstack, entry) => backstack.Push(entry));
        SetBackStack(newBackStack);
    }
    
    private void SetBackStack(IImmutableStack<NavEntry> newBackStack)
    {
        var oldEntry = BackStack.CurrentEntry();
        var oldRoute = oldEntry.Route;
        oldEntry.ViewModel.OnNavigatingFrom();
        var newEntry = newBackStack.CurrentEntry();
        CheckForClosingViewModels(BackStack, newBackStack);
        Navigating?.Invoke(this, new NavigatingEventArgs(oldRoute, oldEntry.ViewModel, newEntry.Route));
        BackStack = newBackStack;
        oldEntry.ViewModel.OnNavigatedFrom();
        newEntry.ViewModel.OnNavigatedTo();
        Navigated?.Invoke(this, new NavigatedEventArgs(oldRoute, newEntry.Route, newEntry.ViewModel));
    }
    
    private void CheckForClosingViewModels(IImmutableStack<NavEntry> oldStack, IImmutableStack<NavEntry> newStack)
    {
        var entriesToClose = oldStack.Except(newStack).ToList();
        foreach (var entry in entriesToClose)
        {
            entry.ViewModel.OnClosing();
        }
    }

    public void Navigate(Route route)
    {
        var newBackStack = BackStack.Push(CreateEntry(route));
        SetBackStack(newBackStack);
    }

    public void GoBack()
    {
        if (!CanGoBack())
            return;
        var newBackStack = BackStack.Pop();
        SetBackStack(newBackStack);
    }

    public void GoBackTo(Route route)
    {
        if (!BackStack.Any(entry => entry.Route.Equals(route)))
            return;
        var newBackStack = BackStack;
        while (!newBackStack.CurrentEntry().Route.Equals(route))
        {
            newBackStack = newBackStack.Pop();
        }
        SetBackStack(newBackStack);
    }

    public void ClearAndSet(Route route) => OverrideBackStack([route]);
    

    public void ReplaceCurrent(Route route)
    {
        var newBackStack = BackStack.Pop();
        newBackStack = newBackStack.Push(CreateEntry(route));
        SetBackStack(newBackStack);
    }
    
    private NavEntry CreateEntry(Route route)
    {
        var vm = _viewModelFactory.CreateViewModel(route, this);
        return new NavEntry(route, vm);
    }
}
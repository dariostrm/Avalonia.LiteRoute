
using System.Collections.Immutable;

namespace Mvvm.NestedNav;

public class Navigator : INavigator
{
    private readonly IViewModelFactory _viewModelFactory;
    
    public IImmutableStack<NavEntry> BackStack { get; private set; }
    public INavigator? ParentNavigator { get; }
    
    public event EventHandler<NavigatingEventArgs>? Navigating;
    public event EventHandler<NavigatedEventArgs>? Navigated;
    
    public Navigator(IViewModelFactory viewModelFactory, Screen initialScreen, INavigator? parentNavigator = null)
    {
        _viewModelFactory = viewModelFactory;
        ParentNavigator = parentNavigator;
        var initialEntry = CreateEntry(initialScreen);
        BackStack = ImmutableStack.Create(initialEntry);
    }

    public virtual bool CanGoBack() => BackStack.Count() > 1;

    public void OverrideBackStack(IEnumerable<Screen> screens)
    {
        var newBackStack = screens
            .Select(CreateEntry)
            .Aggregate(ImmutableStack<NavEntry>.Empty, (backstack, entry) => backstack.Push(entry));
        SetBackStack(newBackStack);
    }
    
    private void SetBackStack(IImmutableStack<NavEntry> newBackStack)
    {
        var oldEntry = BackStack.CurrentEntry();
        var oldScreen = oldEntry.Screen;
        oldEntry.ViewModel.OnNavigatingFrom();
        var newEntry = newBackStack.CurrentEntry();
        CheckForClosingViewModels(BackStack, newBackStack);
        Navigating?.Invoke(this, new NavigatingEventArgs(oldScreen, oldEntry.ViewModel, newEntry.Screen));
        BackStack = newBackStack;
        oldEntry.ViewModel.OnNavigatedFrom();
        newEntry.ViewModel.OnNavigatedTo();
        Navigated?.Invoke(this, new NavigatedEventArgs(oldScreen, newEntry.Screen, newEntry.ViewModel));
    }
    
    private void CheckForClosingViewModels(IImmutableStack<NavEntry> oldStack, IImmutableStack<NavEntry> newStack)
    {
        var entriesToClose = oldStack.Except(newStack).ToList();
        foreach (var entry in entriesToClose)
        {
            entry.ViewModel.OnClosing();
        }
    }

    public void Navigate(Screen screen)
    {
        var newBackStack = BackStack.Push(CreateEntry(screen));
        SetBackStack(newBackStack);
    }

    public void GoBack()
    {
        if (!CanGoBack())
            return;
        var newBackStack = BackStack.Pop();
        SetBackStack(newBackStack);
    }

    public void GoBackTo(Screen screen)
    {
        if (!BackStack.Any(entry => entry.Screen.Equals(screen)))
            return;
        var newBackStack = BackStack;
        while (!newBackStack.CurrentEntry().Screen.Equals(screen))
        {
            newBackStack = newBackStack.Pop();
        }
        SetBackStack(newBackStack);
    }

    public void ClearAndSet(Screen screen) => OverrideBackStack([screen]);
    

    public void ReplaceCurrent(Screen screen)
    {
        var newBackStack = BackStack.Pop();
        newBackStack = newBackStack.Push(CreateEntry(screen));
        SetBackStack(newBackStack);
    }
    
    private NavEntry CreateEntry(Screen screen)
    {
        var vm = _viewModelFactory.CreateViewModel(screen, this);
        return new NavEntry(screen, vm);
    }
}
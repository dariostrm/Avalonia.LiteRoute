using System.Collections.Immutable;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Mvvm.NestedNav;

public class Navigator : INavigator
{
    private CancellationTokenSource? _currentViewModelLoadingCts;
    
    private readonly CompositeDisposable _disposables = new();
    private readonly IViewModelResolver _viewModelResolver;

    private readonly BehaviorSubject<NavBackStack> _stackSubject = new(NavBackStack.Empty);
    
    public IObservable<NavBackStack> BackStack => _stackSubject.AsObservable();
    public NavBackStack BackStackValue => _stackSubject.Value;

    public INavigator? ParentNavigator { get; }
    public bool CanGoBack => BackStackValue.Entries.Count() > 1;

    private readonly Subject<NavigatingEventArgs> _navigatingSubject = new();
    private readonly Subject<NavigatedEventArgs> _navigatedSubject = new();
    
    public IObservable<NavigatingEventArgs> Navigating => _navigatingSubject.AsObservable();
    public IObservable<NavigatedEventArgs> Navigated => _navigatedSubject.AsObservable();
    
    public Navigator(IViewModelResolver viewModelResolver, INavigator? parentNavigator = null)
    {
        _viewModelResolver = viewModelResolver;
        ParentNavigator = parentNavigator;
    }
    
    public void SetBackStack(IEnumerable<Screen> newBackStack)
    {
        
    }

    public void Navigate(Screen screen) => SetBackStack(BackStackValue.Screens.Add(screen));

    public void GoBack() => SetBackStack(BackStackValue.Screens.RemoveLastOrEmpty());

    public void GoBackOrClear()
    {
        if (CanGoBack)
            GoBack();
        else
            Clear();
    }

    public void GoBackTo(Screen screen)
    {
        var screens = BackStackValue.Screens;
        var newBackStack = ImmutableList.Create<Screen>();
        foreach (var s in screens)
        {
            newBackStack = newBackStack.Add(s);
            if (s.Equals(screen))
                break;
        }
        
        SetBackStack(newBackStack);
    }

    public void ClearAndSet(Screen screen) => SetBackStack([screen]);

    public void Clear() => SetBackStack([]);

    public void ReplaceCurrent(Screen screen) => SetBackStack(BackStackValue.Screens.RemoveLastOrEmpty().Add(screen));
    
    private NavBackStack CreateNavBackStackFromScreens(IEnumerable<Screen> screens)
    {
        List<NavEntry> entries = [];
        foreach (var screen in screens)
        {
            var entry = BackStackValue.Entries.FirstOrDefault(e => e.Screen.Equals(screen)) ??
                        new NavEntry(screen, _viewModelResolver.ResolveViewModel(screen));
            entries.Add(entry);
        }
        return new NavBackStack(entries.ToImmutableList());
    }
    
    private async Task LoadNewViewModelAsync(Screen screen, IScreenViewModel vm)
    {
        var newCts = new CancellationTokenSource();
        
        Interlocked.Exchange(ref _currentViewModelLoadingCts, newCts);
        var token = newCts.Token;
        
        try
        {
            vm.Initialize(this, screen);
            await vm.LoadAsync(token);
        }
        catch (OperationCanceledException)
        {
            //expected
        }
    }
    

    public void Dispose()
    {
        _currentViewModelLoadingCts?.Cancel();
        _currentViewModelLoadingCts?.Dispose();
        _disposables.Dispose();
        _stackSubject.Dispose();
        _navigatingSubject.Dispose();
        _navigatedSubject.Dispose();
    }
}
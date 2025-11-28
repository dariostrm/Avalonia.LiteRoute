using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Threading;

namespace Mvvm.NestedNav.Avalonia;

public class NavigationHost : ContentControl
{
    public static readonly StyledProperty<INavigator> NavigatorProperty = AvaloniaProperty.Register<NavigationHost, INavigator>(
        nameof(Navigator));

    public static readonly StyledProperty<Route> InitialRouteProperty = AvaloniaProperty.Register<NavigationHost, Route>(
        nameof(InitialRoute));

    public static readonly StyledProperty<IViewModelFactory> ViewModelFactoryProperty = AvaloniaProperty.Register<NavigationHost, IViewModelFactory>(
        nameof(ViewModelFactory));

    public IViewModelFactory ViewModelFactory
    {
        get => GetValue(ViewModelFactoryProperty);
        set => SetValue(ViewModelFactoryProperty, value);
    }

    private IViewModel? _currentViewModel;

    public static readonly DirectProperty<NavigationHost, IViewModel> CurrentViewModelProperty = AvaloniaProperty.RegisterDirect<NavigationHost, IViewModel>(
        nameof(CurrentViewModel), o => o.CurrentViewModel, (o, v) => o.CurrentViewModel = v);

    public IViewModel CurrentViewModel
    {
        get => _currentViewModel ?? throw new InvalidOperationException("NavigationHost not initialized yet.");
        set => SetAndRaise(CurrentViewModelProperty, ref _currentViewModel!, value);
    }

    public NavigationHost()
    {
        
    }

    public NavigationHost(Route initialRoute, IViewModelFactory viewModelFactory)
        : this()
    {
        InitialRoute = initialRoute;
        ViewModelFactory = viewModelFactory;
    }

    private void SetCurrentViewModel(IViewModel viewModel)
    {
        CurrentViewModel = viewModel;
        Dispatcher.UIThread.Post(() => Content = viewModel);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (InitialRoute is null)
            throw new InvalidOperationException("The " + nameof(InitialRoute) +" has not been set on the navigation host.");
        if (ViewModelFactory is null)
            throw new InvalidOperationException("The " + nameof(ViewModelFactory) + " has not been set on the navigation host.");
        Navigator = new Navigator(ViewModelFactory, InitialRoute, parentNavigator: null);
        SetCurrentViewModel(Navigator.BackStack.CurrentViewModel());
        Navigator.Navigated += OnNavigated;
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        Navigator.Navigated -= OnNavigated;
        base.OnDetachedFromLogicalTree(e);
    }

    private void OnNavigated(object? sender, NavigatedEventArgs e)
    {
        var newViewModel = Navigator.BackStack.CurrentViewModel();
        SetCurrentViewModel(newViewModel);
    }
    

    public INavigator Navigator
    {
        get => GetValue(NavigatorProperty);
        set => SetValue(NavigatorProperty, value);
    }
    public Route InitialRoute
    {
        get => GetValue(InitialRouteProperty);
        set => SetValue(InitialRouteProperty, value);
    }
}
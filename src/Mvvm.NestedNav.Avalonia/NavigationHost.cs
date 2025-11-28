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

    public static readonly StyledProperty<Screen> InitialScreenProperty = AvaloniaProperty.Register<NavigationHost, Screen>(
        nameof(InitialScreen));

    public static readonly StyledProperty<IViewModelFactory> ViewModelFactoryProperty = AvaloniaProperty.Register<NavigationHost, IViewModelFactory>(
        nameof(ViewModelFactory));

    public IViewModelFactory ViewModelFactory
    {
        get => GetValue(ViewModelFactoryProperty);
        set => SetValue(ViewModelFactoryProperty, value);
    }

    private IScreenViewModel? _currentViewModel;

    public static readonly DirectProperty<NavigationHost, IScreenViewModel> CurrentViewModelProperty = AvaloniaProperty.RegisterDirect<NavigationHost, IScreenViewModel>(
        nameof(CurrentViewModel), o => o.CurrentViewModel, (o, v) => o.CurrentViewModel = v);

    public IScreenViewModel CurrentViewModel
    {
        get => _currentViewModel ?? throw new InvalidOperationException("NavigationHost not initialized yet.");
        set => SetAndRaise(CurrentViewModelProperty, ref _currentViewModel!, value);
    }

    public NavigationHost()
    {
        
    }

    public NavigationHost(Screen initialScreen, IViewModelFactory viewModelFactory)
        : this()
    {
        InitialScreen = initialScreen;
        ViewModelFactory = viewModelFactory;
    }

    private void SetCurrentViewModel(IScreenViewModel viewModel)
    {
        CurrentViewModel = viewModel;
        Dispatcher.UIThread.Post(() => Content = viewModel);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (InitialScreen is null)
            throw new InvalidOperationException("The InitialScreen has not been set on the navigation host.");
        if (ViewModelFactory is null)
            throw new InvalidOperationException("The ViewModelFactory has not been set on the navigation host.");
        Navigator = new Navigator(ViewModelFactory, InitialScreen, parentNavigator: null);
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
    public Screen InitialScreen
    {
        get => GetValue(InitialScreenProperty);
        set => SetValue(InitialScreenProperty, value);
    }
}
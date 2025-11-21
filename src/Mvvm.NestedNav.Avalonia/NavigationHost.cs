using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Threading;

namespace Mvvm.NestedNav.Avalonia;

public class NavigationHost : ContentControl, INavigationHost
{
    private readonly CompositeDisposable _disposables = new();

    public static readonly StyledProperty<INavigator> NavigatorProperty = AvaloniaProperty.Register<NavigationHost, INavigator>(
        nameof(Navigator));

    public static readonly StyledProperty<Screen> InitialScreenProperty = AvaloniaProperty.Register<NavigationHost, Screen>(
        nameof(InitialScreen));

    private IViewModel? _currentViewModel;

    public static readonly DirectProperty<NavigationHost, IViewModel?> CurrentViewModelProperty = AvaloniaProperty.RegisterDirect<NavigationHost, IViewModel?>(
        nameof(CurrentViewModel), o => o.CurrentViewModel, (o, v) => o.CurrentViewModel = v);

    public IViewModel? CurrentViewModel
    {
        get => _currentViewModel;
        set => SetAndRaise(CurrentViewModelProperty, ref _currentViewModel, value);
    }

    public NavigationHost()
    {
        
    }

    public NavigationHost(Screen initialScreen)
        : this()
    {
        InitialScreen = initialScreen;
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);
        this.GetObservable(CurrentViewModelProperty)
            .Subscribe(OnCurrentViewModelChanged)
            .DisposeWith(_disposables);
        Navigator = new Navigator(InitialScreen, ViewModelFactory.Instance, parentNavigator: null);
        Navigator.CurrentViewModel
            .Subscribe(OnNewViewModel)
            .DisposeWith(_disposables);
    }

    private void OnCurrentViewModelChanged(IViewModel? vm)
    {
        if (vm == null)
        {
            Console.WriteLine("Warning: CurrentViewModel is null.");
        }
        Dispatcher.UIThread.Post(() => Content = vm);
    }

    private void OnNewViewModel(IViewModel? vm)
    {
        CurrentViewModel = vm;
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        _disposables.Dispose();
        base.OnDetachedFromLogicalTree(e);
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
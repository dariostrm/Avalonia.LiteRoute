using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Mvvm.NestedNav;

public abstract class ScreenViewModel : ObservableValidator, IScreenViewModel
{
    private readonly BehaviorSubject<ViewModelLifecycleState> _lifecycleStateSubject 
        = new(ViewModelLifecycleState.Created);

    public IObservable<ViewModelLifecycleState> LifecycleState => _lifecycleStateSubject.AsObservable();
    
    private INavigator? _navigator;
    public virtual INavigator Navigator
    {
        get => _navigator ?? throw new InvalidOperationException("The ViewModel has not been initialized yet.");
        private set => _navigator = value;
    }
    
    private Screen? _screen;
    public virtual Screen Screen 
    {
        get => _screen ?? throw new InvalidOperationException("The ViewModel has not been initialized yet.");
        private set => _screen = value;
    }
    
    public virtual void Initialize(INavigator navigator, Screen screen)
    {
        Navigator = navigator;
        Screen = screen;
    }

    public virtual Task LoadAsync(CancellationToken cancellationToken = default)
    {
        _lifecycleStateSubject.OnNext(ViewModelLifecycleState.Loading);
        return Task.CompletedTask;
    }

    public virtual void OnNavigatedTo()
    {
        _lifecycleStateSubject.OnNext(ViewModelLifecycleState.Active);
    }

    public virtual void OnNavigatingFrom() {}

    public virtual void OnNavigatedFrom()
    {
        _lifecycleStateSubject.OnNext(ViewModelLifecycleState.Inactive);
    }
    
    public virtual void OnClosing()
    {
        _lifecycleStateSubject.OnNext(ViewModelLifecycleState.Closing);
    }

}
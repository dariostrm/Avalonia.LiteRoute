using CommunityToolkit.Mvvm.ComponentModel;

namespace Mvvm.NestedNav;

public abstract class ViewModelBase : ObservableValidator, IViewModel
{
    private INavigator? _navigator;
    public virtual INavigator Navigator
    {
        get => _navigator ?? throw new InvalidOperationException("The ViewModel has not been initialized yet.");
        private set => _navigator = value;
    }
    
    public virtual void OnInitialize(INavigator navigator)
    {
        Navigator = navigator;
    }

    public virtual void OnActivate() {}

    public virtual void OnMoveToBackground() {}
    
    public virtual void OnDestroy() {}
}
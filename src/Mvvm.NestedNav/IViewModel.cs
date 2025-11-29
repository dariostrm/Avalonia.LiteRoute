using System.ComponentModel;

namespace Mvvm.NestedNav;

public interface IViewModel
{
    void OnInitialize(INavigator navigator);
    
    void OnActivate();
    void OnMoveToBackground();
    void OnDestroy();
}
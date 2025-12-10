namespace Avalonia.LiteRoute;

public interface IViewModelFactory
{
    IViewModel CreateViewModel(Route route, INavigator navigator);
}
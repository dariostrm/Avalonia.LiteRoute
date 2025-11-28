using Microsoft.Extensions.DependencyInjection;

namespace Mvvm.NestedNav;

public class ViewModelFactory(IServiceProvider serviceProvider) : IViewModelFactory
{
    private readonly Dictionary<Type, Func<IServiceProvider, IViewModel>> _factories = new();

    public void Register<TRoute, TViewModel>()
        where TRoute : Route
        where TViewModel : IViewModel
    {
        _factories[typeof(TRoute)] = sp => ActivatorUtilities.CreateInstance<TViewModel>(sp);
    }
    
    public IViewModel CreateViewModel(Route route, INavigator navigator)
    {
        var routeType = route.GetType();
        if (_factories.TryGetValue(routeType, out var factory))
        {
            var vm = factory(serviceProvider);
            vm.Initialize(navigator, route);
            return vm;
        }

        throw new InvalidOperationException($"No ViewModel registered for Route type {routeType.FullName}");
    }
    
}
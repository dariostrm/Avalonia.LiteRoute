using Microsoft.Extensions.DependencyInjection;

namespace Mvvm.NestedNav;

public class ServiceProviderViewModelResolver(IServiceProvider serviceProvider) : IViewModelResolver
{
    private readonly Dictionary<Type, Func<IServiceProvider, IScreenViewModel>> _factories = new();

    public void Register<TScreen, TViewModel>()
        where TScreen : Screen
        where TViewModel : IScreenViewModel
    {
        _factories[typeof(TScreen)] = sp => ActivatorUtilities.CreateInstance<TViewModel>(sp);
    }


    public IScreenViewModel ResolveViewModel<TScreen>() where TScreen : Screen
    {
        var screenType = typeof(TScreen);
        if (_factories.TryGetValue(screenType, out var factory))
        {
            return factory(serviceProvider);
        }

        throw new InvalidOperationException($"No ViewModel registered for Screen type {screenType.FullName}");
    }
    
    public IScreenViewModel ResolveViewModel(Screen screen)
    {
        var screenType = screen.GetType();
        if (_factories.TryGetValue(screenType, out var factory))
        {
            return factory(serviceProvider);
        }

        throw new InvalidOperationException($"No ViewModel registered for Screen type {screenType.FullName}");
    }
}
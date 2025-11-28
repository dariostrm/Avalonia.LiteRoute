using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Mvvm.NestedNav.Avalonia.Samples.Screens;
using Mvvm.NestedNav.Avalonia.Samples.ViewModels;
using Mvvm.NestedNav.Avalonia.Samples.Views;

namespace Mvvm.NestedNav.Avalonia.Samples;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<HomeViewModel>();
        serviceCollection.AddSingleton<IViewModelFactory>(sp =>
        {
            var factory = new ViewModelFactory(sp);
            factory.Register<HomeScreen, HomeViewModel>();
            factory.Register<DetailsScreen, DetailsViewModel>();
            factory.Register<ProfileScreen, ProfileViewModel>();
            factory.Register<SettingsScreen, SettingsViewModel>();
            return factory;
        });
        serviceCollection.AddSingleton<MainViewModel>();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            var mainVm = serviceProvider.GetRequiredService<MainViewModel>();
            desktop.MainWindow = new MainWindow()
            {
                DataContext = mainVm,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
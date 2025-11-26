using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Mvvm.NestedNav.Dialogs.Avalonia;

public class DialogTheme : Styles
{
    public DialogTheme(IServiceProvider? serviceProvider = null)
    {
        AvaloniaXamlLoader.Load(serviceProvider, this);
    }
}
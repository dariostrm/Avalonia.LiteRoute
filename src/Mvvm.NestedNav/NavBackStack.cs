using System.Collections.Immutable;

namespace Mvvm.NestedNav;

public static class NavBackStack
{
    public static NavEntry CurrentEntry(this IImmutableStack<NavEntry> entries)
    {
        return entries.Peek();
    }
    
    public static Screen CurrentScreen(this IImmutableStack<NavEntry> entries)
    {
        return entries.CurrentEntry().Screen;
    }
    
    public static IScreenViewModel CurrentViewModel(this IImmutableStack<NavEntry> entries) 
    {
        return entries.CurrentEntry().ViewModel;
    }
}
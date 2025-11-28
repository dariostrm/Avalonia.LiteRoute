using System.Collections.Immutable;

namespace Mvvm.NestedNav;

public static class NavBackStack
{
    public static NavEntry CurrentEntry(this IImmutableStack<NavEntry> entries)
    {
        return entries.Peek();
    }
    
    public static Route CurrentRoute(this IImmutableStack<NavEntry> entries)
    {
        return entries.CurrentEntry().Route;
    }
    
    public static IViewModel CurrentViewModel(this IImmutableStack<NavEntry> entries) 
    {
        return entries.CurrentEntry().ViewModel;
    }
}
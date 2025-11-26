using System.Collections.Immutable;

namespace Mvvm.NestedNav;

public record NavBackStack(IImmutableList<NavEntry> Entries)
{
    public NavEntry? CurrentEntry => IsEmpty ? null : Entries[^1];
    public Screen? CurrentScreen => CurrentEntry?.Screen;
    public IScreenViewModel? CurrentViewModel => CurrentEntry?.ViewModel;
    public IImmutableList<Screen> Screens => Entries.Select(e => e.Screen).ToImmutableList();
    public bool IsEmpty => Count == 0;
    public int Count => Entries.Count;
    
    public static NavBackStack Empty => new([]);
}

public static class ImmutableListNavBackStackExtensions
{
    public static IImmutableList<T> RemoveLast<T>(this IImmutableList<T> list)
    {
        if (list.Count == 0)
            throw new InvalidOperationException("Cannot remove last element from an empty list.");
        
        return list.RemoveAt(list.Count - 1);
    }
    
    public static IImmutableList<T> RemoveLastOrEmpty<T>(this IImmutableList<T> list)
    {
        if (list.Count == 0)
            return list;
        
        return list.RemoveAt(list.Count - 1);
    }
}
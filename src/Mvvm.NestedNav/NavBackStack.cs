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

public static class NavBackStackExtensions
{
    public static NavBackStack Push(this NavBackStack stack, NavEntry entry)
    {
        return new NavBackStack(stack.Entries.Add(entry));
    }
    
    public static NavBackStack TryPop(this NavBackStack stack, out NavEntry? poppedEntry)
    {
        if (stack.IsEmpty)
        {
            poppedEntry = null;
            return stack;
        }

        poppedEntry = stack.CurrentEntry;
        return new NavBackStack(stack.Entries.RemoveAt(stack.Count - 1));
    }
}
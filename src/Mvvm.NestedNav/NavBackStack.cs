using System.Collections.Immutable;

namespace Mvvm.NestedNav;

public record NavBackStack(IImmutableStack<NavEntry> Stack) : INavBackStack
{
    public NavEntry? CurrentEntry => Stack.IsEmpty ? null : Stack.Peek();
    public Screen? CurrentScreen => CurrentEntry?.Screen;
    public IScreenViewModel? CurrentViewModel => CurrentEntry?.ViewModel;
}
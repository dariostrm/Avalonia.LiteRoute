using System.Collections.Immutable;

namespace Mvvm.NestedNav;

public interface INavBackStack
{
    IImmutableStack<NavEntry> Stack { get; }
    NavEntry? CurrentEntry { get; }
    Screen? CurrentScreen { get; }
    IScreenViewModel? CurrentViewModel { get; }
}
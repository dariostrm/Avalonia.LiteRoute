namespace Mvvm.NestedNav.Dialogs;

public record DialogScreen(string Title) : Route;

public record DialogScreen<TDialogResult>(string Title) : DialogScreen(Title);
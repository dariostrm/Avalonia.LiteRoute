namespace Mvvm.NestedNav.Dialogs;

public record DialogScreen(string Title) : Screen;

public record DialogScreen<TDialogResult>(string Title) : DialogScreen(Title);
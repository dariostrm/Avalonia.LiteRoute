namespace Mvvm.NestedNav.Avalonia.Samples.Screens;

public record HomeScreen : Screen;
public record ProfileScreen : Screen;
public record SettingsScreen : Screen;
public record DetailsScreen(string Message) : Screen;
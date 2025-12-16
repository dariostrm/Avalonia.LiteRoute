namespace Avalonia.LiteRoute.Tests;

public record TestRoute : Route;
public record TestRoute2 : Route;
public record TestRoute3 : Route;

public record TestRouteWithParam(string Param) : Route;
public record TestRouteWithTwoParams(int Id, string Name) : Route;

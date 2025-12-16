using NSubstitute;

namespace Avalonia.LiteRoute.Tests;

public class NavigatorTests
{
    private readonly IViewModelFactory _factory = Substitute.For<IViewModelFactory>();
    
    [Fact]
    public void Constructor_Should_CreateNavEntryForInitialRoute()
    {
        // Arrange
        var initialRoute = new TestRoute();
        var testViewModel = Substitute.For<IViewModel>();
        _factory.CreateViewModel(initialRoute, Arg.Any<INavigator>()).Returns(testViewModel);

        // Act
        var navigator = new Navigator(_factory, initialRoute);

        // Assert
        Assert.Single(navigator.BackStack);
        Assert.Equal(initialRoute, navigator.CurrentEntry.Route);
        Assert.Same(testViewModel, navigator.CurrentEntry.ViewModel);
        Assert.Same(navigator.CurrentEntry, navigator.BackStack.Peek());
        _factory.CreateViewModel(initialRoute, navigator).Received(1);
    }
}
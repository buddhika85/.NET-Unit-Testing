using FluentAssertions;

namespace GameLibrary.UnitTests;

public class PlayerTests
{
    [Fact]
    public void IncreaseLevel_WhenCalled_HasExpectedLevel()
    {
        // Arrange
        Player sut = SetupPlayer(level: 1);

        // Act
        sut.IncreaseLevel();

        // Assert
        sut.Level.Should().Be(2);
        sut.Level.Should().BeGreaterThan(1);
        sut.Level.Should().BeGreaterThanOrEqualTo(2);
        sut.Level.Should().BePositive();
        sut.Level.Should().NotBe(1);
        sut.Level.Should().BeInRange(2, 100);
    }

    [Fact]
    public void Greet_ValidGreeting_ReturnsGreetingWithName()
    {
        // Arrange
        var sut = SetupPlayer(name: "Alice");

        // Act
        var result = sut.Greet("Hello");

        // Assert
        result.Should().Be("Hello, Alice!");
        result.Should().Contain("Alice");
        result.Should().EndWith("Alice!");
        result.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Constructor_OnNewInstance_SetsJoinDate()
    {
        // Arrange
        var currentDate = DateTime.Now;

        // Act
        var sut = SetupPlayer(joinDate: currentDate);

        // Assert
        sut.JoinDate.Should().Be(currentDate);
        sut.JoinDate.Should().BeCloseTo(currentDate, TimeSpan.FromMilliseconds(500));
        sut.JoinDate.Should().BeWithin(TimeSpan.FromMilliseconds(500)).Before(currentDate);
    }

    [Fact]
    public void AddItemToInventory_WithValidItem_AddsTheItem()
    {
        // Arrange
        var sut = SetupPlayer();
        var item = new InventoryItem(101, "Sword", "A sharp blade.");

        // Act
        sut.AddItemToInventory(item);

        // Assert
        sut.InventoryItems.Should().HaveCount(1);
        sut.InventoryItems.Should().NotBeEmpty();
        sut.InventoryItems.Should().Contain(item);
        sut.InventoryItems.Should().ContainSingle(
            item => item.Id == 101 && item.Name == "Sword");
    }

    [Fact]
    public void Greet_NullOrEmptyGreeting_ThrowsArgumentException()
    {
        // Arrange
        var sut = SetupPlayer();

        // Act
        Action act = () => sut.Greet("");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void IncreaseLevel_WhenCalled_RaisesLevelUpEvent()
    {
        // Arrange
        var sut = SetupPlayer();
        using var monitoredSut = sut.Monitor();

        // Act
        sut.IncreaseLevel();

        // Assert
        monitoredSut.Should().Raise(nameof(sut.LevelUp));
    }

    private static Player SetupPlayer(string name = "Alice", int level = 1, DateTime? joinDate = null) =>
        new(name, level, joinDate ?? DateTime.Now);

}


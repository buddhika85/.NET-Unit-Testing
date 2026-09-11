using FluentAssertions;

namespace GameLibrary.UnitTests;

public class PlayerTests
{
    [Fact]
    public void IncreaseLevel_WhenCalled_HasExpectedLevel()
    {
        // arrange
        var sut = new Player("Alice", 1, DateTime.Now);
        var monitor = sut.Monitor();

        // act
        sut.IncreaseLevel();

        // assert
        sut.Level.Should().Be(2, "a level 1 player should become level 2 after increasing level once");
        monitor.Should().Raise(nameof(sut.LevelUp))
        .WithSender(sut)
       .WithArgs<EventArgs>();
    }

    [Fact]
    public void IncreaseLevel_WhenCalled_RaisesLevelUpEvent()
    {
        // arrange
        var sut = new Player("Alice", 1, DateTime.Now);
        using var monitor = sut.Monitor();

        // act
        sut.IncreaseLevel();

        // assert
        monitor.Should().Raise(nameof(sut.LevelUp))
            .WithSender(sut)
            .WithArgs<EventArgs>();
    }

    [Fact]
    public void Greet_ValidStringGreeting_RetunrsGreetingWithName()
    {
        // arrange
        var sut = new Player("Alice", 1, DateTime.Now);

        // act
        string greeting = "Good Morning";
        var result = sut.Greet(greeting);

        // assert
        result.Should().Be($"{greeting}, {sut.Name}!");
    }

    [Fact]
    public void Constructor_OnNewInstance_SetsPassedJoinDate()
    {
        // arrange
        var joinDate = DateTime.Now;

        // act
        var sut = new Player("Alice", 1, joinDate);

        // assert    
        sut.JoinDate.Should().Be(joinDate);
    }

    [Fact]
    public void AddItemToInventory_WithValidItem_AddsToIventoryList()
    {
        // arrange
        var sut = new Player("Alice", 1, DateTime.Now);
        var inventoryItem = new InventoryItem(1, "Sword", "A sharp blade");

        // act
        sut.AddItemToInventory(inventoryItem);

        // assert
        sut.InventoryItems.Should().HaveCount(1);
        sut.InventoryItems.Should().ContainSingle(x =>
            x.Id == inventoryItem.Id &&
            string.Equals(x.Name, inventoryItem.Name) &&
            string.Equals(x.Description, inventoryItem.Description));
        sut.InventoryItems.First().Should().BeEquivalentTo(inventoryItem);
    }

    [Fact]
    public void Greet_NullOrEmptyGreeting_ThrowsArgumentException()
    {
        // arrange
        var sut = new Player("Alice", 1, DateTime.Now);

        // act
        Action act = () => sut.Greet("");

        // assert
        act.Should().Throw<ArgumentException>()
                    .WithParameterName("greeting");
    }

    // arrange

    // act

    // assert
}

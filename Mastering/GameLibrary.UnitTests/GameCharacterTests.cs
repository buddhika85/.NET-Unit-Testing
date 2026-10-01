using AutoFixture;
using FluentAssertions;

namespace GameLibrary.UnitTests;

public class GameCharacterTests
{
    private readonly IFixture fixture = new Fixture();

    [Fact]
    public void Heal_WithPositiveNumber_IncreasesHealth()
    {
        // Arrange
        var sut = fixture.Create<GameCharacter>();   //new GameCharacter("Character 1", 1, 1);
        var initialHealth = sut.Health;
        var points = fixture.Create<int>();         //       3;

        // Act
        sut.Heal(points);

        // Assert
        sut.Health.Should().Be(initialHealth + points);
    }

    [Fact]
    public void AddItemToInventory_WithValidItem_IncreasesInventoryCount()
    {
        // Arrange
        var sut = fixture.Create<GameCharacter>();
        var item = fixture.Create<InventoryItem>();     //new InventoryItem(1, "Item 1", "Description 1");
        var initialCount = sut.Inventory.Count;

        // Act
        sut.AddItemToInventory(item);

        // Assert
        sut.Inventory.Count.Should().Be(initialCount + 1);
    }

    [Fact]
    public void AddItemToInventory_WhenInventoryIsFull_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = fixture.Create<GameCharacter>();

        var items = fixture.CreateMany<InventoryItem>(10 - sut.Inventory.Count);

        foreach (var item in items)
        {
            sut.AddItemToInventory(item);
        }

        var newItem = fixture.Create<InventoryItem>(); // The 11th item

        // Act
        Action act = () => sut.AddItemToInventory(newItem);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}


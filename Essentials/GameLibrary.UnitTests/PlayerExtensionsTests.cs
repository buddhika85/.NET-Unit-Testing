using FluentAssertions;

namespace GameLibrary.UnitTests;

public class PlayerExtensionsTests
{
    [Fact]
    public void ToDto_WhenCalledOnAPlayerObject_ReturnsPlayerDtoObject()
    {
        // arrange
        var player = new Player("Alice", 1, DateTime.Now);
        player.AddItemToInventory(new InventoryItem(101, "Sword", "A sharp blade"));

        // act
        var dto = player.ToDto();

        // assert
        dto.Should().BeOfType<PlayerDto>();
        dto.Should().BeEquivalentTo(player, x =>
            x.Excluding(s => s.InventoryItems)
            .Excluding(s => s.ExperiencePoints));
        // dto.Name.Should().Be(player.Name);
        // dto.Level.Should().Be(player.Level);
        // dto.JoinDate.Should().Be(player.JoinDate);
    }
}

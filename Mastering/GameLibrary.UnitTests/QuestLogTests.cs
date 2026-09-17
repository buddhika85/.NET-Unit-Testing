using FluentAssertions;

namespace GameLibrary.UnitTests;

public class QuestLogTests
{
    [Fact]
    public void AddQuest_WhenCalled_AddsQuestToLog()
    {
        // Arrange
        var sut = new QuestLog("Initial Quest");

        // Act
        var result = sut.AddQuest("Second Quest");

        // Assert
        result.Should().BeTrue();
        sut.Quests.Should().HaveCount(2);
        sut.Quests.Should().Contain("Second Quest");
    }
}

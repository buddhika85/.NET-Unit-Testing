using FluentAssertions;

namespace GameLibrary.UnitTests;

public class QuestRewardCalculatorTests
{
    [Theory]
    [InlineData(1, 1.0, 100)]
    [InlineData(3, 2.0, 600)]
    public void CalculateQuestReward_GivenDifficultyAndRewardMultiplier_RetunrsExpectedReward(
        int questDifficulty,
        double rewardMultiplier,
        int expectedReward)
    {
        // Arrange
        var sut = new QuestRewardCalculator(rewardMultiplier);

        // Act
        var result = sut.CalculateQuestReward(questDifficulty);

        // Assert
        result.Should().Be(expectedReward);
    }
}

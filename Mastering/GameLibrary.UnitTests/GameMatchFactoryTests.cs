using FluentAssertions;
using Moq;

namespace GameLibrary.UnitTests;

public class GameMatchFactoryTests
{
    [Fact]
    public void CreateMatch_WhenOnWeekend_CreatesMatch()
    {
        // Arrange
        var knownSaturday = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero);
        var timeProviderStub = new Mock<TimeProvider>();
        timeProviderStub.Setup(x => x.GetUtcNow()).Returns(knownSaturday);
        var sut = new GameMatchFactory(timeProviderStub.Object);

        // Act
        GameMatch match = sut.CreateMatch("Alice");

        // Assert
        match.Should().NotBeNull();
        match.Player1.Should().Be("Alice");
        match.MatchState.Should().Be(MatchState.WaitingForOpponent);
        match.CreatedTime.Should().Be(knownSaturday);
    }

    [Fact]
    public void CreateMatch_WhenOnWeekday_Throws()
    {
        // Arrange
        var knownMonday = new DateTimeOffset(2023, 5, 22, 0, 0, 0, TimeSpan.Zero);
        var timeProviderStub = new Mock<TimeProvider>();
        timeProviderStub.Setup(x => x.GetUtcNow()).Returns(knownMonday);
        var sut = new GameMatchFactory(timeProviderStub.Object);

        // Act
        Action act = () => sut.CreateMatch("Alice");

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}


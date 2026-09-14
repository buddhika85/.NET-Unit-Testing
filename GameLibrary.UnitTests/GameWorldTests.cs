using FluentAssertions;
using Moq;

namespace GameLibrary.UnitTests;

public class GameWorldTests
{

    [Fact]
    public void GetPlayerReport_PlayerExists_ReturnsPlayerReportDto()
    {
        // Arrange

        // Act

        // Assert

    }

    [Fact]
    public void GetPlayerReport_WhenCalled_ReturnsPlayerReportDto()
    {
        // arrange
        var playerName = "Alice";
        var playerLevel = 1;
        var joinDate = new DateTime(2024, 1, 1);
        var gamePlayed = 1;
        var totalScore = 10;
        var averageScore = (double)totalScore / gamePlayed;
        var player = new Player(playerName, playerLevel, joinDate);

        var mockStats = new Mock<IPlayerStatisticsService>();
        mockStats.Setup(x =>
            x.GetPlayerStatistics(playerName))
            .Returns(
                new PlayerStatistics
                {
                    PlayerName = playerName,
                    GamesPlayed = gamePlayed,
                    TotalScore = totalScore
                }
                );
        var sut = new GameWorld(mockStats.Object);


        // act
        var result = sut.GetPlayerReport(player);

        // assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PlayerReportDto>();
        result.Should().BeEquivalentTo(new PlayerReportDto(
           playerName,
           playerLevel,
           joinDate,
           gamePlayed,
           totalScore,
           averageScore
        ));

        mockStats.Verify(x => x.GetPlayerStatistics(playerName), Times.Once);
    }

    [Fact]
    public void RecordPlayerGameWin_CalledWithScoreAndPlayer_CallsUpdatePlayerStatistics()
    {
        // Arrange
        var player = new Player("Alice", 1, new DateTime(2024, 1, 1));
        var playerStats = new PlayerStatistics
        {
            PlayerName = player.Name,
            GamesPlayed = 10,
            TotalScore = 100
        };
        var statServiceMock = new Mock<IPlayerStatisticsService>();
        statServiceMock.Setup(x => x.GetPlayerStatistics(player.Name)).Returns(playerStats);

        var sut = new GameWorld(statServiceMock.Object);
        var winScore = 15;

        // Act
        sut.RecordPlayerGameWin(player, winScore);

        // Assert    
        playerStats.Should().BeEquivalentTo(new
        {
            PlayerName = player.Name,
            GamesPlayed = 11,
            TotalScore = 115
        });
        statServiceMock.Verify(x => x.UpdatePlayerStatistics(
            It.Is<PlayerStatistics>(ps =>
                ps.PlayerName == "Alice" &&
                ps.GamesPlayed == 11 &&
                ps.TotalScore == 115)
            ), Times.Once);
    }
}

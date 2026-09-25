using FluentAssertions;
using Matchmaker.Api.Services;
using MatchMaker.Api.Contracts.Dtos;
using MatchMaker.Api.Entities;
using MatchMaker.Api.Repositories;
using Microsoft.Extensions.Logging;
using MatchMaker.Api.Contracts;
using Moq;

namespace MatchMaker.Api.Tests.Services;

public class GameMatcherTests
{
    [Fact]
    public async Task MatchPlayerAsync_WhenPlayerHasAMatch_ReturnsThatMatchDto()
    {
        // arrange
        var playerId = "Alice";
        var playersGameMatch = new GameMatch(playerId);

        var repoStub = new Mock<IGameMatchRepository>();
        repoStub.Setup(x => x.FindMatchForPlayerAsync(playerId))
            .ReturnsAsync(playersGameMatch);

        var loggerMock = new Mock<ILogger<GameMatcher>>();

        var sut = new GameMatcher(repoStub.Object, loggerMock.Object);

        var request = new JoinMatchRequest(playerId);

        var gameDtoExpected = playersGameMatch.ToGameMatchResponse();

        // act
        var result = await sut.MatchPlayerAsync(request);

        // assert
        result.Should().BeEquivalentTo(gameDtoExpected);

        repoStub.Verify(x => x.FindMatchForPlayerAsync(playerId), Times.Once);
        repoStub.Verify(x => x.FindOpenMatchAsync(), Times.Never);
        repoStub.Verify(x => x.CreateMatchAsync(It.IsAny<GameMatch>()), Times.Never);
        repoStub.Verify(x => x.UpdateMatchAsync(It.IsAny<GameMatch>()), Times.Never);
    }


    [Fact]
    public async Task MatchPlayerAsync_WhenPlayerDoesNotHaveAMatchAndTheresAnOpenMatch_OpenMatchPlayer2SetAndReturnsIt()
    {
        // arrange
        var playerId = "Alice";
        GameMatch? playersGameMatch = null;
        GameMatch openGameMatch = new("Bob");
        GameMatch expectedGameMatch = new("Bob");
        expectedGameMatch.SetPlayer2(playerId);
        var expectedGameMatchDto = expectedGameMatch.ToGameMatchResponse();
        var request = new JoinMatchRequest(playerId);

        var repoStub = new Mock<IGameMatchRepository>();
        repoStub.Setup(x => x.FindMatchForPlayerAsync(playerId)).ReturnsAsync(playersGameMatch);
        repoStub.Setup(x => x.FindOpenMatchAsync()).ReturnsAsync(openGameMatch);

        var logger = new Mock<ILogger<GameMatcher>>();
        var sut = new GameMatcher(repoStub.Object, logger.Object);

        // act
        var result = await sut.MatchPlayerAsync(request);

        // assert
        result.Should().BeEquivalentTo(expectedGameMatchDto);
        repoStub.Verify(x => x.FindMatchForPlayerAsync(playerId), Times.Once);
        repoStub.Verify(x => x.FindOpenMatchAsync(), Times.Once);
        repoStub.Verify(x => x.UpdateMatchAsync(openGameMatch), Times.Once);

        repoStub.Verify(x => x.CreateMatchAsync(It.IsAny<GameMatch>()), Times.Never);
    }

    [Fact]
    public void MatchPlayerAsync_WhenPlayerDoesNotHaveAMatchAndTheresNoOpenMatch_CreatesNewMatchAsPlayer1AndReturnsIt()
    {
        // arrange

        // act

        // assert
    }
}

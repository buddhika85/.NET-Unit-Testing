using FluentAssertions;
using Matchmaker.Api.Services;
using MatchMaker.Api.Contracts.Dtos;
using MatchMaker.Api.Entities;
using MatchMaker.Api.Repositories;
using Microsoft.Extensions.Logging;
using MatchMaker.Api.Contracts;
using Moq;
using MatchMaker.Api.Exceptions;

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
    public async Task MatchPlayerAsync_WhenPlayerDoesNotHaveAMatchAndTheresNoOpenMatch_CreatesNewMatchAsPlayer1AndReturnsIt()
    {
        // arrange        
        var repoMock = new Mock<IGameMatchRepository>();
        var loggerStub = new Mock<ILogger<GameMatcher>>();

        repoMock.Setup(x => x.FindMatchForPlayerAsync("Alice")).ReturnsAsync((GameMatch?)null);
        repoMock.Setup(x => x.FindOpenMatchAsync()).ReturnsAsync((GameMatch?)null);

        var expectedResult = new GameMatch("Alice").ToGameMatchResponse();

        var sut = new GameMatcher(repoMock.Object, loggerStub.Object);

        // act
        var result = await sut.MatchPlayerAsync(new JoinMatchRequest("Alice"));

        // assert
        result.Should().BeEquivalentTo(expectedResult);

        repoMock.Verify(x => x.FindMatchForPlayerAsync("Alice"), Times.Once);
        repoMock.Verify(x => x.FindOpenMatchAsync(), Times.Once);
        repoMock.Verify(x => x.CreateMatchAsync(
            It.Is<GameMatch>(x =>
            x.Player1 == "Alice" &&
            x.State == GameMatchState.WaitingForOpponent &&
            x.Player2 == null &&
            x.ServerIpAddress == null &&
            x.ServerPort == null
            )
            ),
            Times.Once);
        repoMock.Verify(x => x.UpdateMatchAsync(It.IsAny<GameMatch>()), Times.Never);
    }

    [Fact]
    public async Task AssignServerToMatchAsync_WhenMatchReadyMatchWithIdAvailable_UpdatesServerDetailsAndReturnsGameMatchResponse()
    {
        // arrange
        var matchId = 1;
        var resultMatch = CreateMatch();
        resultMatch.SetPlayer2("Bob");
        var ipAddress = "192.168.1.1";
        var port = 5555;

        var repoMock = new Mock<IGameMatchRepository>();
        var loggerStub = new Mock<ILogger<GameMatcher>>();
        repoMock.Setup(x => x.FindMatchByIdAsync(matchId)).ReturnsAsync(resultMatch);

        var sut = new GameMatcher(repoMock.Object, loggerStub.Object);

        // act
        var result = await sut.AssignServerToMatchAsync(matchId, new AssignServerToMatchRequest(ipAddress, port));

        // assert
        result.Should().BeEquivalentTo(
            new
            {
                Player1 = "Alice",
                Player2 = "Bob",
                State = GameMatchState.ServerReady.ToString(),
                IpAddress = ipAddress,
                Port = port
            });

        repoMock.Verify(x => x.FindMatchByIdAsync(matchId), Times.Once);
        repoMock.Verify(x => x.UpdateMatchAsync(resultMatch), Times.Once);
    }

    [Fact]
    public async Task AssignServerToMatchAsync_WhenMatchWithIdUnavailable_ThrowsMatchNotFoundException()
    {
        // arrange
        var matchId = 1;
        var mockRepo = new Mock<IGameMatchRepository>();
        var loggerStub = new Mock<ILogger<GameMatcher>>();

        mockRepo.Setup(x => x.FindMatchByIdAsync(matchId)).ReturnsAsync((GameMatch?)null);
        var sut = new GameMatcher(mockRepo.Object, loggerStub.Object);

        // act
        Func<Task> act = async () => await sut.AssignServerToMatchAsync(matchId,
            new AssignServerToMatchRequest("192.192.192.192", 5555));

        // assert
        await act.Should().ThrowAsync<MatchNotFoundException>()
            .WithMessage($"Match with id {matchId} was not found.");

        mockRepo.Verify(x => x.FindMatchByIdAsync(matchId), Times.Once);
    }


    private static GameMatch CreateMatch(string player1 = "Alice")
    {
        return new GameMatch(player1);
    }
}

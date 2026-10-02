using MatchMaker.Domain.Entities;
using MatchMaker.Application.Repositories;
using Microsoft.Extensions.Logging;
using MatchMaker.Application.MatchMaking.MatchPlayer;
using Xunit;
using Moq;
using AutoFixture;
using MatchMaker.Application.Contracts;
using FluentAssertions;
using MatchMaker.Application.Contracts.Dtos;


namespace MatchMaker.UnitTests.Application.MatchMaking;

public class MatchPlayerTests
{
    private readonly IFixture fixture = new Fixture();

    [Fact]
    public async Task Handle_WhenPlayerMatchExists_ReturnsMatch()
    {
        // arrange
        var repoStub = new Mock<IGameMatchRepository>();
        var loggerStub = new Mock<ILogger<MatchPlayerCommandHandler>>();

        var playerId = fixture.Create<string>();
        var gameMatch = new GameMatch(playerId);
        repoStub.Setup(x => x.FindMatchForPlayerAsync(playerId)).ReturnsAsync(gameMatch);

        var sut = new MatchPlayerCommandHandler(repoStub.Object, loggerStub.Object);


        var expected = gameMatch.ToGameMatchResponse();

        // act
        var result = await sut.Handle(
                            new MatchPlayerCommand(playerId),
                            fixture.Create<CancellationToken>());

        // assert
        result.Should().Be(expected);

        repoStub.Verify(x => x.FindMatchForPlayerAsync(playerId), Times.Once);
        repoStub.Verify(x => x.FindOpenMatchAsync(), Times.Never);
        repoStub.Verify(x => x.CreateMatchAsync(It.IsAny<GameMatch>()), Times.Never);
        repoStub.Verify(x => x.UpdateMatchAsync(It.IsAny<GameMatch>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPlayerMatchNotExistsButOpenMatchExists_SetPlayer2AndReturnsMatch()
    {
        // Arrange 
        var repoStub = new Mock<IGameMatchRepository>();
        var loggerStub = new Mock<ILogger<MatchPlayerCommandHandler>>();

        var playerId = fixture.Create<string>();
        var match = new GameMatch(fixture.Create<string>());
        repoStub.Setup(x => x.FindMatchForPlayerAsync(playerId)).ReturnsAsync((GameMatch?)null);
        repoStub.Setup(x => x.FindOpenMatchAsync()).ReturnsAsync(match);

        var sut = new MatchPlayerCommandHandler(repoStub.Object, loggerStub.Object);

        // Act
        var result = await sut.Handle(
            new MatchPlayerCommand(playerId),
            fixture.Create<CancellationToken>());

        // Assert
        result.Should().BeEquivalentTo(
            new GameMatchResponse(
                match.Id,
                match.Player1,
                playerId,
                "MatchReady",
                null,
                null)
            );


        repoStub.Verify(x => x.FindMatchForPlayerAsync(playerId), Times.Once);
        repoStub.Verify(x => x.FindOpenMatchAsync(), Times.Once);
        repoStub.Verify(x => x.CreateMatchAsync(It.IsAny<GameMatch>()), Times.Never);
        repoStub.Verify(x => x.UpdateMatchAsync(It.IsAny<GameMatch>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenPlayerMatchNotExistsAndOpenMatchNotExists_CreatesNewMatchAndReturnsMatch()
    {
        // Arrange
        var repoStub = new Mock<IGameMatchRepository>();
        var loggerStub = new Mock<ILogger<MatchPlayerCommandHandler>>();

        var playerId = fixture.Create<string>();
        repoStub.Setup(x => x.FindMatchForPlayerAsync(playerId)).ReturnsAsync((GameMatch?)null);
        repoStub.Setup(x => x.FindOpenMatchAsync()).ReturnsAsync((GameMatch?)null);

        var sut = new MatchPlayerCommandHandler(repoStub.Object, loggerStub.Object);

        // Act
        var result = await sut.Handle(
            new MatchPlayerCommand(playerId),
            fixture.Create<CancellationToken>());

        // Assert
        result.Should().BeEquivalentTo(new
        {
            Player1 = playerId,
            State = "WaitingForOpponent"
        });


        repoStub.Verify(x => x.FindMatchForPlayerAsync(playerId), Times.Once);
        repoStub.Verify(x => x.FindOpenMatchAsync(), Times.Once);
        repoStub.Verify(x => x.CreateMatchAsync(It.IsAny<GameMatch>()), Times.Once);
        repoStub.Verify(x => x.UpdateMatchAsync(It.IsAny<GameMatch>()), Times.Never);
    }

}

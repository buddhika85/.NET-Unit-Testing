using MatchMaker.Domain.Entities;
using NSubstitute;
using MatchMaker.Application.Repositories;
using Microsoft.Extensions.Logging;
using MatchMaker.Application.MatchMaking.MatchPlayer;
using Xunit;
using Moq;
using AutoFixture;
using MatchMaker.Application.Contracts;
using FluentAssertions;


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


    #region "Utilities"

    private static GameMatch CreateMatch()
    {
        return new("P1");
    }

    private static IGameMatchRepository CreateRepositoryStub()
    {
        return Substitute.For<IGameMatchRepository>();
    }

    private static ILogger<MatchPlayerCommandHandler> CreateLoggerStub()
    {
        return Substitute.For<ILogger<MatchPlayerCommandHandler>>();
    }

    #endregion
}

using FluentAssertions;
using Matchmaker.Api.Services;
using MatchMaker.Api.Contracts.Dtos;
using MatchMaker.Api.Controllers;
using MatchMaker.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MatchMaker.Api.Tests.Controllers;

public class MatchesControllerTests
{
    [Fact]
    public async Task GetMatchForPlayerAsync_WhenGameExistsForPlayer_ReturnsGameMatchResponse()
    {
        // arrange
        var gameMatherStub = GetGameMatherStub();
        var expected = new GameMatchResponse(1,
                "Alice",
                null,
                GameMatchState.WaitingForOpponent.ToString(),
                null,
                null);
        gameMatherStub.Setup(x => x.GetMatchForPlayerAsync("Alice"))
            .ReturnsAsync(expected);
        var sut = new MatchesController(gameMatherStub.Object);

        // act
        ActionResult<GameMatchResponse> result =
            await sut.GetMatchForPlayerAsync("Alice");

        // assert
        result.Should().BeOfType<ActionResult<GameMatchResponse>>();
        result.Value.Should().BeSameAs(expected);

        gameMatherStub.Verify(x => x.GetMatchForPlayerAsync("Alice"), Times.Once);
    }

    [Fact]
    public async Task GetMatchForPlayerAsync_WhenNoGameExistsForPlayer_ReturnsNotFound404()
    {
        // arrange
        var gameMatcherStub = GetGameMatherStub();
        gameMatcherStub.Setup(x => x.GetMatchForPlayerAsync("Alice"))
            .ReturnsAsync((GameMatchResponse?)null);
        var sut = new MatchesController(gameMatcherStub.Object);

        // act
        var result = await sut.GetMatchForPlayerAsync("Alice");

        // assert
        result.Should().BeOfType<ActionResult<GameMatchResponse>>();
        result.Value.Should().BeNull();
        result.Result.Should().BeOfType<NotFoundResult>();

        gameMatcherStub.Verify(x => x.GetMatchForPlayerAsync("Alice"), Times.Once);
    }

    private static Mock<IGameMatcher> GetGameMatherStub() => new();
}

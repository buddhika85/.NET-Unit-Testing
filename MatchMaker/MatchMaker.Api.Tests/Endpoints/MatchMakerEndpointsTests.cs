using FluentAssertions;
using Matchmaker.Api.Services;
using MatchMaker.Api.Contracts.Dtos;
using MatchMaker.Api.Endpoints;
using MatchMaker.Api.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace MatchMaker.Api.Tests.Endpoints;

public class MatchMakerEndpointsTests
{
    [Fact]
    public async Task GetMatchForPlayerAsync_WhenNoGame_ReturnsNotFound()
    {
        // arrange
        var matcherStub = new Mock<IGameMatcher>();
        matcherStub.Setup(x => x.GetMatchForPlayerAsync(It.IsAny<string>()))
            .ReturnsAsync((GameMatchResponse?)null);

        // act
        var result = await MatchMakerEndpoints.GetMatchForPlayerAsync(matcherStub.Object,
            It.IsAny<string>());

        // assert
        result.Should().BeOfType<Results<NotFound, Ok<GameMatchResponse>>>();
        result.Result.Should().BeOfType<NotFound>();

        matcherStub.Verify(x => x.GetMatchForPlayerAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetMatchForPlayerAsync_WhenGameExists_ReturnsGameWithInOkResult()
    {
        // arrange
        var matcherStub = new Mock<IGameMatcher>();
        var expected = new GameMatchResponse(1,
                    It.IsAny<string>(),
                    null,
                    GameMatchState.WaitingForOpponent.ToString(),
                    null,
                    null);
        matcherStub.Setup(x => x.GetMatchForPlayerAsync(It.IsAny<string>()))
            .ReturnsAsync(expected);

        // act
        var result = await MatchMakerEndpoints.GetMatchForPlayerAsync(matcherStub.Object,
                                                    It.IsAny<string>());

        // assert
        result.Should().BeOfType<Results<NotFound, Ok<GameMatchResponse>>>();
        Ok<GameMatchResponse> okResult = result.Result.Should().BeOfType<Ok<GameMatchResponse>>().Subject;
        okResult.Value.Should().BeSameAs(expected);

        matcherStub.Verify(x => x.GetMatchForPlayerAsync(It.IsAny<string>()), Times.Once);
    }
}

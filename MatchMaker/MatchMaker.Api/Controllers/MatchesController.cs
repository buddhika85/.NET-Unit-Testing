using Matchmaker.Api.Services;
using MatchMaker.Api.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers;

[ApiController]
[Route("matches")]
public class MatchesController : ControllerBase
{
    private readonly IGameMatcher gameMatcher;

    public MatchesController(IGameMatcher gameMatcher)
    {
        this.gameMatcher = gameMatcher;
    }

    [HttpPost]
    public async Task<GameMatchResponse> JoinMatchAsync(JoinMatchRequest request)
    {
        return await gameMatcher.JoinMatchAsync(request);
    }

    [HttpGet]
    public async Task<ActionResult<GameMatchResponse>> GetMatchForPlayerAsync([FromQuery] string playerId)
    {
        var gameMatchResponse = await gameMatcher.GetMatchForPlayerAsync(playerId);
        return gameMatchResponse is null ? NotFound() : gameMatchResponse;
    }

    [HttpGet("{matchId}")]
    public async Task<ActionResult<GameMatchResponse>> GetMatchByIdAsync(int matchId)
    {
        return await gameMatcher.GetMatchByIdAsync(matchId);
    }

    [HttpPut("{matchId}")]
    public async Task<IActionResult> AssignServerToMatchAsync(int matchId, AssignServerToMatchRequest request)
    {
        await gameMatcher.AssignServerToMatchAsync(matchId, request);
        return NoContent();
    }

    [HttpDelete("{matchId}")]
    public async Task<IActionResult> DeleteMatchAsync(int matchId)
    {
        await gameMatcher.DeleteMatchAsync(matchId);
        return NoContent();
    }
}
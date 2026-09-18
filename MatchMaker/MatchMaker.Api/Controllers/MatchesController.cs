using System.Net;
using MatchMaker.Api.Contracts;
using MatchMaker.Api.Contracts.Dtos;
using MatchMaker.Api.Entities;
using MatchMaker.Api.Exceptions;
using MatchMaker.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers;

[ApiController]
[Route("matches")]
public class MatchesController : ControllerBase
{
    private readonly IGameMatchRepository repository;
    private readonly ILogger<MatchesController> logger;

    public MatchesController(IGameMatchRepository repository, ILogger<MatchesController> logger)
    {
        this.repository = repository;
        this.logger = logger;        
    }

    [HttpPost]
    public async Task<GameMatchResponse> JoinMatchAsync(JoinMatchRequest request)
    {
        string playerId = request.PlayerId;

        logger.LogInformation("Matching player {PlayerId}...", playerId);

        // Is the player already assigned to a match?
        GameMatch? match = await repository.FindMatchForPlayerAsync(playerId);

        if (match is null)
        {
            // Is there an open match he can join?
            match = await repository.FindOpenMatchAsync();

            if (match is null)
            {
                // Create a new match
                match = new GameMatch
                {
                    Player1 = playerId,
                    State = GameMatchState.WaitingForOpponent
                };

                await repository.CreateMatchAsync(match);
            }
            else
            {
                // Assign to open match
                match.Player2 = playerId;
                match.State = GameMatchState.MatchReady;
                await repository.UpdateMatchAsync(match);
            }

            logger.LogInformation("{PlayerId} assigned to match {MatchId}.", playerId, match.Id);
        }
        else
        {
            logger.LogInformation("{PlayerId} already assigned to match {MatchId}.", playerId, match.Id);
        }

        return match.ToGameMatchResponse();        
    }

    [HttpGet]
    public async Task<ActionResult<GameMatchResponse>> GetMatchForPlayerAsync([FromQuery] string playerId)
    {
        var match = await repository.FindMatchForPlayerAsync(playerId);
        return match is null ? NotFound() : match.ToGameMatchResponse();
    }

    [HttpGet("{matchId}")]
    public async Task<ActionResult<GameMatchResponse>> GetMatchByIdAsync(int matchId)
    {
        var match = await repository.FindMatchByIdAsync(matchId) ?? throw new MatchNotFoundException(matchId);

        return match.ToGameMatchResponse();        
    }    

    [HttpPut("{matchId}")]
    public async Task<IActionResult> AssignServerToMatchAsync(int matchId, AssignServerToMatchRequest request)
    {
        var match = await repository.FindMatchByIdAsync(matchId) ?? throw new MatchNotFoundException(matchId);

        if (!IPAddress.TryParse(request.IpAddress, out var parsedIpAddress))
        {
            throw new InvalidIpAddressException(request.IpAddress);
        }

        if (request.Port < IPEndPoint.MinPort || request.Port > IPEndPoint.MaxPort)
        {
            throw new InvalidPortException(request.Port);
        }

        if (match.State != GameMatchState.MatchReady)
        {
            throw new MatchNotReadyException($"Cannot set server details for match in state {match.State}.");
        }

        match.ServerIpAddress = parsedIpAddress;
        match.ServerPort = request.Port;
        match.State = GameMatchState.ServerReady;

        await repository.UpdateMatchAsync(match);

        return NoContent();
    }

    [HttpDelete("{matchId}")]
    public async Task<IActionResult> DeleteMatchAsync(int matchId)
    {
        bool matchDeleted = await repository.DeleteMatchAsync(matchId);

        if (!matchDeleted)
        {
            throw new MatchNotFoundException(matchId);
        }

        return NoContent();
    }    
}
using MatchMaker.Api.Contracts;
using MatchMaker.Api.Contracts.Dtos;
using MatchMaker.Api.Entities;
using MatchMaker.Api.Exceptions;
using MatchMaker.Api.Repositories;

namespace Matchmaker.Api.Services;

public class GameMatcher : IGameMatcher
{
    private readonly IGameMatchRepository repository;
    private readonly ILogger<GameMatcher> logger;

    public GameMatcher(IGameMatchRepository repository, ILogger<GameMatcher> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

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
                match = new GameMatch(playerId);

                await repository.CreateMatchAsync(match);
            }
            else
            {
                // Assign to open match
                match.SetPlayer2(playerId);
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

    public async Task<GameMatchResponse?> GetMatchForPlayerAsync(string playerId)
    {
        var match = await repository.FindMatchForPlayerAsync(playerId);
        return match?.ToGameMatchResponse();
    }

    public async Task<GameMatchResponse> GetMatchByIdAsync(int matchId)
    {
        var match = await repository.FindMatchByIdAsync(matchId) ??
            throw new MatchNotFoundException(matchId);
        return match.ToGameMatchResponse();
    }

    public async Task<GameMatchResponse> AssignServerToMatchAsync(int matchId, AssignServerToMatchRequest request)
    {
        var match = await repository.FindMatchByIdAsync(matchId) ?? throw new MatchNotFoundException(matchId);

        match.SetServerDetails(request.IpAddress, request.Port);

        await repository.UpdateMatchAsync(match);

        return match.ToGameMatchResponse();     // for unit testing this will be used, not in controller
    }

    public async Task<bool> DeleteMatchAsync(int matchId)
    {
        bool matchDeleted = await repository.DeleteMatchAsync(matchId);
        if (!matchDeleted)
        {
            throw new MatchNotFoundException(matchId);
        }
        return matchDeleted;
    }
}

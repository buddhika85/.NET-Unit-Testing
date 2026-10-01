using MatchMaker.Application.Contracts;
using MatchMaker.Application.Contracts.Dtos;
using MatchMaker.Application.Repositories;
using MatchMaker.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Application.MatchMaking.MatchPlayer;

public class MatchPlayerCommandHandler : IRequestHandler<MatchPlayerCommand, GameMatchResponse>
{
    private readonly IGameMatchRepository repository;
    private readonly ILogger<MatchPlayerCommandHandler> logger;

    public MatchPlayerCommandHandler(IGameMatchRepository repository, ILogger<MatchPlayerCommandHandler> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    public async Task<GameMatchResponse> Handle(MatchPlayerCommand request, CancellationToken cancellationToken)
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
}

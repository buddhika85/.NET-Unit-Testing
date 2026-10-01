using MatchMaker.Application.Contracts;
using MatchMaker.Application.Contracts.Dtos;
using MatchMaker.Application.Exceptions;
using MatchMaker.Application.Repositories;
using MediatR;

namespace MatchMaker.Application.MatchMaking.AssignServerToMatch;

public class AssignServerToMatchCommandHandler : IRequestHandler<AssignServerToMatchCommand, GameMatchResponse>
{
    private readonly IGameMatchRepository repository;

    public AssignServerToMatchCommandHandler(IGameMatchRepository repository)
    {
        this.repository = repository;
    }

    public async Task<GameMatchResponse> Handle(AssignServerToMatchCommand request, CancellationToken cancellationToken)
    {
        var match = await repository.FindMatchByIdAsync(request.MatchId) ?? throw new MatchNotFoundException(request.MatchId);

        match.SetServerDetails(request.IpAddress, request.Port);

        await repository.UpdateMatchAsync(match);

        return match.ToGameMatchResponse();
    }
}

using MatchMaker.Application.Contracts;
using MatchMaker.Application.Contracts.Dtos;
using MatchMaker.Application.Exceptions;
using MatchMaker.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Application.MatchMaking.GetMatchById;

public class GetMatchByIdQueryHandler : IRequestHandler<GetMatchByIdQuery, GameMatchResponse>
{
    private readonly IGameMatchRepository repository;

    public GetMatchByIdQueryHandler(IGameMatchRepository repository, ILogger<GetMatchByIdQuery> logger)
    {
        this.repository = repository;
    }

    public async Task<GameMatchResponse> Handle(GetMatchByIdQuery request, CancellationToken cancellationToken)
    {
        var match = await repository.FindMatchByIdAsync(request.MatchId) ?? throw new MatchNotFoundException(request.MatchId);
        return match.ToGameMatchResponse();
    }
}

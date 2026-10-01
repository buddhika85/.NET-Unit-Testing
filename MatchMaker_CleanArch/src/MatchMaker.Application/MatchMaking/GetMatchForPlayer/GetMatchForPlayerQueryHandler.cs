using MatchMaker.Application.Contracts;
using MatchMaker.Application.Contracts.Dtos;
using MatchMaker.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Application.MatchMaking.GetMatchForPlayer;

public class GetMatchForPlayerQueryHandler : IRequestHandler<GetMatchForPlayerQuery, GameMatchResponse?>
{
    private readonly IGameMatchRepository repository;

    public GetMatchForPlayerQueryHandler(IGameMatchRepository repository)
    {
        this.repository = repository;
    }

    public async Task<GameMatchResponse?> Handle(GetMatchForPlayerQuery request, CancellationToken cancellationToken)
    {
        var match = await repository.FindMatchForPlayerAsync(request.PlayerId);
        return match?.ToGameMatchResponse();
    }
}

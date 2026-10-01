using MatchMaker.Application.Contracts.Dtos;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Application.Contracts;

public static class DtoExtensions
{
    public static GameMatchResponse ToGameMatchResponse(this GameMatch match)
    {
        return new GameMatchResponse(match.Id, match.Player1, match.Player2, match.State.ToString(), match.ServerIpAddress?.ToString(), match.ServerPort);
    }
}
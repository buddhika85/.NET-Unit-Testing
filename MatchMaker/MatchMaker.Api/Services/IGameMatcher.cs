using MatchMaker.Api.Contracts.Dtos;

namespace Matchmaker.Api.Services;

public interface IGameMatcher
{
    Task<GameMatchResponse> AssignServerToMatchAsync(int matchId, AssignServerToMatchRequest request);
    Task<bool> DeleteMatchAsync(int matchId);
    Task<GameMatchResponse> GetMatchByIdAsync(int matchId);
    Task<GameMatchResponse?> GetMatchForPlayerAsync(string playerId);
    Task<GameMatchResponse> MatchPlayerAsync(JoinMatchRequest request);
}

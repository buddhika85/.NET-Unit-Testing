using MatchMaker.Api.Entities;

namespace MatchMaker.Api.Repositories;

public interface IGameMatchRepository
{
    Task CreateMatchAsync(GameMatch match);
    Task<GameMatch?> FindMatchForPlayerAsync(string playerId);
    Task<GameMatch?> FindOpenMatchAsync();
    Task<GameMatch?> FindMatchByIdAsync(int matchId);
    Task UpdateMatchAsync(GameMatch match);
    Task<bool> DeleteMatchAsync(int matchId);
}

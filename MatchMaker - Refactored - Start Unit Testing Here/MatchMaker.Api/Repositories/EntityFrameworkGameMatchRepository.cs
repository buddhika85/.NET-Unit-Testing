using MatchMaker.Api.Data;
using MatchMaker.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace MatchMaker.Api.Repositories;

public class EntityFrameworkGameMatchRepository : IGameMatchRepository
{
    private readonly MatchMakerDbContext dbContext;

    public EntityFrameworkGameMatchRepository(MatchMakerDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<GameMatch?> FindMatchForPlayerAsync(string playerId)
    {
        return await dbContext.Matches
                              .Where(match => match.Player1 == playerId 
                                           || match.Player2 == playerId)
                              .FirstOrDefaultAsync();
    }

    public async Task<GameMatch?> FindOpenMatchAsync()
    {
        return await dbContext.Matches
                              .Where(match => match.State == GameMatchState.WaitingForOpponent)
                              .FirstOrDefaultAsync();
    }

    public async Task<GameMatch?> FindMatchByIdAsync(int matchId) 
        => await dbContext.Matches.FindAsync(matchId);

    public async Task CreateMatchAsync(GameMatch match)
    {
        dbContext.Matches.Add(match);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateMatchAsync(GameMatch match)
    {
        dbContext.Update(match);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteMatchAsync(int matchId)
    {
        var deletedRows = await dbContext.Matches
                    .Where(match => match.Id == matchId)
                    .ExecuteDeleteAsync();

        return deletedRows > 0;
    }    
}
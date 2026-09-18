namespace GameLibrary;

public class GameMatchFactory
{
    private int nextMatchId = 1;
    private readonly TimeProvider timeProvider;

    public GameMatchFactory(TimeProvider timeProvider)
    {
        this.timeProvider = timeProvider;
    }

    public GameMatch CreateMatch(string player)
    {
        var now = timeProvider.GetUtcNow();

        if (now.DayOfWeek != DayOfWeek.Saturday && now.DayOfWeek != DayOfWeek.Sunday)
        {
            throw new InvalidOperationException("Can't create matches on weekdays!");
        }

        return new GameMatch
        {
            Id = nextMatchId++,
            Player1 = player,
            MatchState = MatchState.WaitingForOpponent,
            CreatedTime = now
        };
    }
}

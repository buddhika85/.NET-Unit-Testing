namespace MatchMaker.Api.Exceptions;

public class MatchNotFoundException : Exception
{
    public MatchNotFoundException(int matchId)
    {
        this.MatchId = matchId;
    }

    public int MatchId { get; }

    public override string Message => $"Match with id {this.MatchId} was not found.";
}
namespace MatchMaker.Api.Exceptions;

public class MatchNotReadyException : Exception
{
    private readonly string reason;

    public MatchNotReadyException(string reason)
    {
        this.reason = reason;
    }

    public override string Message => reason;
}
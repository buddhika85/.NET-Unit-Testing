using System.Net;

namespace MatchMaker.Api.Exceptions;

public class InvalidPortException : Exception
{
    public InvalidPortException(int port)
    {
        this.Port = port;
    }

    public int Port { get; }

    public override string Message => $"The port must be between {IPEndPoint.MinPort} and {IPEndPoint.MaxPort}.";
}
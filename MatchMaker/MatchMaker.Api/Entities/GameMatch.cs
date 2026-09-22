using System.Net;
using MatchMaker.Api.Exceptions;

namespace MatchMaker.Api.Entities;

public class GameMatch
{

    public int Id { get; private set; }

    public string Player1 { get; private set; }

    public string? Player2 { get; private set; }

    public GameMatchState State { get; private set; }

    public IPAddress? ServerIpAddress { get; private set; }

    public int? ServerPort { get; private set; }

    public GameMatch(string player1)
    {
        ArgumentException.ThrowIfNullOrEmpty(player1);

        Player1 = player1;
        State = GameMatchState.WaitingForOpponent;
    }

    public void SetPlayer2(string player2)
    {
        ArgumentException.ThrowIfNullOrEmpty(player2);
        Player2 = player2;
        State = GameMatchState.MatchReady;
    }

    public void SetServerDetails(string ipAddress, int port)
    {
        ArgumentException.ThrowIfNullOrEmpty(ipAddress);

        if (!IPAddress.TryParse(ipAddress, out var parsedIpAddress))
        {
            throw new InvalidIpAddressException(ipAddress);
        }

        if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
        {
            throw new InvalidPortException(port);
        }

        if (State != GameMatchState.MatchReady)
        {
            throw new MatchNotReadyException($"Cannot set server details for match in state {State}.");
        }

        ServerIpAddress = parsedIpAddress;
        ServerPort = port;
        State = GameMatchState.ServerReady;
    }

}

public enum GameMatchState
{
    WaitingForOpponent,
    MatchReady,
    ServerReady
}
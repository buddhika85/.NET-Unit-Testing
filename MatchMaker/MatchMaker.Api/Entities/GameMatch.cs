using System.Net;

namespace MatchMaker.Api.Entities;

public class GameMatch
{
    public int Id { get; set; }

    public required string Player1 { get; set; }

    public string? Player2 { get; set; }

    public GameMatchState State { get; set; }

    public IPAddress? ServerIpAddress { get; set; }

    public int? ServerPort { get; set; }
}

public enum GameMatchState
{
    WaitingForOpponent,
    MatchReady,
    ServerReady
}
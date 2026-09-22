namespace MatchMaker.Api.Contracts.Dtos;

public record GameMatchResponse(int Id, string Player1, string? Player2, string State, string? IpAddress, int? Port);
public record JoinMatchRequest(string PlayerId);
public record AssignServerToMatchRequest(string IpAddress, int Port);
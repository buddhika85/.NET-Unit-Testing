using MatchMaker.Application.Contracts.Dtos;
using MediatR;

namespace MatchMaker.Application.MatchMaking.AssignServerToMatch;

public record AssignServerToMatchCommand(int MatchId, string IpAddress, int Port) : IRequest<GameMatchResponse>;
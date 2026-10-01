using MatchMaker.Application.Contracts.Dtos;
using MediatR;

namespace MatchMaker.Application.MatchMaking.MatchPlayer;

public record MatchPlayerCommand(string PlayerId) : IRequest<GameMatchResponse>;
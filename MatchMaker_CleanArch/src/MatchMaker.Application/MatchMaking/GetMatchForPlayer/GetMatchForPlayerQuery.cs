using MatchMaker.Application.Contracts.Dtos;
using MediatR;

namespace MatchMaker.Application.MatchMaking.GetMatchForPlayer;

public record GetMatchForPlayerQuery(string PlayerId) : IRequest<GameMatchResponse?>;
using MatchMaker.Application.Contracts.Dtos;
using MediatR;

namespace MatchMaker.Application.MatchMaking.GetMatchById;

public record GetMatchByIdQuery(int MatchId) : IRequest<GameMatchResponse>;
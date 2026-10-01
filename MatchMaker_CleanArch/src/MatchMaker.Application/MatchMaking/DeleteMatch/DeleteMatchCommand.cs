using MediatR;

namespace MatchMaker.Application.MatchMaking.DeleteMatch;

public record DeleteMatchCommand(int MatchId) : IRequest<bool>;
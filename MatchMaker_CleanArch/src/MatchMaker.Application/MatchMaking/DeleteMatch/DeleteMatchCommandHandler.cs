using MatchMaker.Application.Exceptions;
using MatchMaker.Application.Repositories;
using MediatR;

namespace MatchMaker.Application.MatchMaking.DeleteMatch;

public class DeleteMatchCommandHandler : IRequestHandler<DeleteMatchCommand, bool>
{
    private readonly IGameMatchRepository repository;

    public DeleteMatchCommandHandler(IGameMatchRepository repository)
    {
        this.repository = repository;
    }

    public async Task<bool> Handle(DeleteMatchCommand request, CancellationToken cancellationToken)
    {
        bool matchDeleted = await repository.DeleteMatchAsync(request.MatchId);

        if (!matchDeleted)
        {
            throw new MatchNotFoundException(request.MatchId);
        }

        return matchDeleted;
    }
}

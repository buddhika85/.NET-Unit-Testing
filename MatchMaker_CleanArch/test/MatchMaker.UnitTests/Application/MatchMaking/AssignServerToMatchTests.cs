using MatchMaker.Domain.Entities;
using NSubstitute;
using MatchMaker.Application.Repositories;

namespace MatchMaker.UnitTests.Application.MatchMaking;

public class AssignServerToMatchTests
{


    #region "Utilities"

    private static GameMatch CreateMatch()
    {
        return new("P1");
    }

    private static IGameMatchRepository CreateRepositoryStub()
    {
        return Substitute.For<IGameMatchRepository>();
    }

    #endregion
}

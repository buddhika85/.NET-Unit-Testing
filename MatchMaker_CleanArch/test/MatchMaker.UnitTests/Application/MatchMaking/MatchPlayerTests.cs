using MatchMaker.Domain.Entities;
using NSubstitute;
using MatchMaker.Application.Repositories;
using Microsoft.Extensions.Logging;
using MatchMaker.Application.MatchMaking.MatchPlayer;


namespace MatchMaker.UnitTests.Application.MatchMaking;

public class MatchPlayerTests
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

    private static ILogger<MatchPlayerCommandHandler> CreateLoggerStub()
    {
        return Substitute.For<ILogger<MatchPlayerCommandHandler>>();
    }

    #endregion
}

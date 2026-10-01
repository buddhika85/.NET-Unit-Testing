using FluentAssertions;
using MatchMaker.Domain.Entities;
using MatchMaker.Domain.Exceptions;
using Xunit;

namespace MatchMaker.UnitTests.Domain.Entities;

public class GameMatchTests
{
    #region Constructor   

    [Fact]
    public void Constructor_WhenPlayer1IsNull_ThrowsArgumentNullException()
    {
        // Arrange
        string player1 = null!;

        // Act
        Action act = () => new GameMatch(player1);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_WhenCalled_SetsInitialProperties()
    {
        // Arrange
        var player1 = "P1";

        // Act
        GameMatch sut = new(player1);

        // Assert
        sut.Player1.Should().Be(player1);
        sut.State.Should().Be(GameMatchState.WaitingForOpponent);
    }

    #endregion

    #region SetPlayer2

    [Fact]
    public void SetPlayer2_WhenPlayer2IsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var player1 = "P1";
        string player2 = null!;

        GameMatch sut = new(player1);

        // Act
        Action act = () => sut.SetPlayer2(player2);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void SetPlayer2_WhenCalled_SetsExpectedProperties()
    {
        // Arrange
        var player1 = "P1";
        var player2 = "P2";
        GameMatch sut = new(player1);

        // Act
        sut.SetPlayer2(player2);

        // Assert
        sut.Player2.Should().Be(player2);
        sut.State.Should().Be(GameMatchState.MatchReady);        
    }

    #endregion    

    #region SetServerDetails

    [Fact]
    public void SetServerDetails_InvalidIpAddress_ThrowsInvalidIpAddressException()
    {
        // Arrange
        GameMatch sut = new("P1");
        string invalidIpAddress = "invalid ip address";
        int port = 1234;

        // Act
        Action act = () => sut.SetServerDetails(invalidIpAddress, port);

        // Assert
        act.Should().Throw<InvalidIpAddressException>();
    }

    [Fact]
    public void SetServerDetails_MatchReady_SetsExpectedProperties()
    {
        // Arrange
        GameMatch sut = new("P1");
        sut.SetPlayer2("P2");
        string ipAddress = "192.168.0.1";
        int port = 1234;

        // Act
        sut.SetServerDetails(ipAddress, port);

        // Assert
        sut.ServerIpAddress!.ToString().Should().Be(ipAddress);
        sut.ServerPort.Should().Be(port);
        sut.State.Should().Be(GameMatchState.ServerReady);        
    }

    #endregion    
}

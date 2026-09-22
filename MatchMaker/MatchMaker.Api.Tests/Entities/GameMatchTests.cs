using FluentAssertions;
using MatchMaker.Api.Entities;
using MatchMaker.Api.Exceptions;

namespace MatchMaker.Api.Tests.Entities;

public class GameMatchTests
{
    [Fact]
    public void Constructor_WhenPlayer1IsNull_ThrowsArgumentException()
    {
        // arrange
        string player1 = null!;

        // act
        Action act = () => new GameMatch(player1);

        // assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("player1"); ;
    }

    [Fact]
    public void Constructor_WhenCalledWithNonNullPlayer1_SetsPlayer1WithWaitingForOpponentState()
    {
        // arrange
        // act
        var sut = new GameMatch("Alice");

        // assert
        sut.Player1.Should().Be("Alice");
        sut.Player2.Should().BeNull();
        sut.State.Should().Be(GameMatchState.WaitingForOpponent);
        sut.ServerIpAddress.Should().BeNull();
        sut.ServerPort.Should().BeNull();
    }

    [Fact]
    public void SetPlayer2_WhenPlayer2IsNull_ThrowsArgumentException()
    {
        // arrange
        string? player2 = null!;
        var sut = new GameMatch("Alice");

        // act
        Action act = () => sut.SetPlayer2(player2);

        // assert
        act.Should().Throw<ArgumentException>().WithParameterName("player2");
    }

    [Fact]
    public void SetPlayer2_WhenPlayer2IsNonNull_AssignsPlayer2AndSetStateMatchReady()
    {
        // Arrange
        var sut = new GameMatch("Alice");

        // Act
        sut.SetPlayer2("Bob");

        // Assert
        sut.Player2.Should().NotBeNull();
        sut.Player2.Should().Be("Bob");
        sut.State.Should().Be(GameMatchState.MatchReady);
    }

    [Fact]
    public void SetServerDetails_WhenIpAddressIsNull_ThrowsArgumentException()
    {
        // arrange
        var sut = GetTestGameMatch();
        string ipAddress = null!;
        var port = 100;

        // act
        Action act = () => sut.SetServerDetails(ipAddress, port);

        // assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("invalid ip address")]
    [InlineData("192 168 1 1")]
    [InlineData("192_168_1_1")]
    [InlineData("192.168.1 1")]
    public void SetServerDetails_WhenIpAddressIsInvalid_ThrowsInvalidIpAddressException(
        string invalidIpAddress)
    {
        // arrange
        var sut = GetTestGameMatch();
        var validPort = 100;

        // act
        Action act = () => sut.SetServerDetails(invalidIpAddress, validPort);

        // assert
        act.Should().Throw<InvalidIpAddressException>();
    }

    [Theory]        // valid range 0 to 65535
    [InlineData(-1)]
    [InlineData(65536)]
    public void SetServerDetails_WhenInvalidPort_ThrowsInvalidPortException(int invalidPort)
    {
        // arrange
        var sut = GetTestGameMatch();
        var validIpAddress = "192.168.1.1";

        // act
        Action act = () => sut.SetServerDetails(validIpAddress, invalidPort);

        // assert
        act.Should().Throw<InvalidPortException>();
    }

    [Fact]
    public void SetServerDetails_WhenStateIsNotMatchReady_ThrowsMatchNotReadyException()
    {
        // arrange
        var sut = new GameMatch("Alice");
        var validIpAddress = "192.168.1.1";
        var validPort = 100;

        // act
        Action act = () => sut.SetServerDetails(validIpAddress, validPort);

        // assert
        act.Should().Throw<MatchNotReadyException>()
            .WithMessage($"Cannot set server details for match in state {sut.State}.");
    }

    [Fact]
    public void SetServerDetails_WhenIpAddressIsValidAndPortValidAndMatchReady_SetsIpPortAndServerReadyState()
    {
        // arrange
        var sut = GetTestGameMatch();
        var validIpAddress = "192.168.1.1";
        var validPort = 100;

        // act
        sut.SetServerDetails(validIpAddress, validPort);

        // assert
        sut.ServerIpAddress!.ToString().Should().Be(validIpAddress);
        sut.ServerPort.Should().Be(validPort);
        sut.State.Should().Be(GameMatchState.ServerReady);
    }

    private GameMatch GetTestGameMatch(string name = "Alice")
    {
        var gameMatch = new GameMatch(name);
        gameMatch.SetPlayer2("Bob");        // state becomes = GameMatchState.MatchReady;
        return gameMatch;
    }
}

using FluentAssertions;
using MatchMaker.Api.ErrorHandling;
using MatchMaker.Api.Exceptions;
using Moq;

namespace MatchMaker.Api.Tests.ErrorHandling;

public class ErrorMapperTests
{
    public static IEnumerable<object[]> GetTestData() =>
    [
        [new InvalidIpAddressException(It.IsAny<string>()), 400],
        [new InvalidPortException(It.IsAny<int>()), 400],
        [new MatchNotReadyException(It.IsAny<string>()), 400],
        [new MatchNotFoundException(It.IsAny<int>()), 404],
        [null!, 500]
    ];

    public static TheoryData<Exception?, int> GetTheoryData() => new()
    {
        { new InvalidIpAddressException(It.IsAny<string>()), 400 },
        { new InvalidPortException(It.IsAny<int>()), 400 },
        { new MatchNotReadyException(It.IsAny<string>()), 400 },
        { new MatchNotFoundException(It.IsAny<int>()), 404 },
        { null!, 500 }
    };

    [Theory]
    [MemberData(nameof(GetTestData))] // either MemberData or TheoryData
    [MemberData(nameof(GetTheoryData))]
    public void MapStatusCode_WhenExceptionPassed_ReturnsStatusCode(Exception? exception, int expectedStatusCode)
    {
        // arrange       

        // act
        var resultStatusCode = ErrorMapper.MapStatusCode(exception);

        // assert
        resultStatusCode.Should().Be(expectedStatusCode);
    }
}

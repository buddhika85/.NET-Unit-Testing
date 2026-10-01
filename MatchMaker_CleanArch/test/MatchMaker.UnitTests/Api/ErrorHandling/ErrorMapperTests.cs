using FluentAssertions;
using MatchMaker.Api.ErrorHandling;
using MatchMaker.Application.Exceptions;
using MatchMaker.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace MatchMaker.UnitTests.Api.ErrorHandling;

public class ErrorMapperTests
{
    [Theory]
    [MemberData(nameof(Data))]
    public void MapStatusCode_WhenCalled_ReturnsExpectedStatusCode(Exception exception, int expected)
    {
        // Arrange & Act
        int actual = ErrorMapper.MapStatusCode(exception);

        // Assert
        actual.Should().Be(expected);
    }

    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new InvalidIpAddressException("1.2.3.4"), StatusCodes.Status400BadRequest },
            new object[] { new InvalidPortException(999), StatusCodes.Status400BadRequest },
            new object[] { new MatchNotReadyException("invalid"), StatusCodes.Status400BadRequest },
            new object[] { new MatchNotFoundException(123), StatusCodes.Status404NotFound },
            new object[] { new Exception(), StatusCodes.Status500InternalServerError }
        };    
}

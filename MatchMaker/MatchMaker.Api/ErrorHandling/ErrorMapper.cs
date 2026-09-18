using MatchMaker.Api.Exceptions;

namespace MatchMaker.Api.ErrorHandling;

public static class ErrorMapper
{
    public static int MapStatusCode(Exception? exception)
    {
        return exception switch
        {
            InvalidIpAddressException => StatusCodes.Status400BadRequest,
            InvalidPortException => StatusCodes.Status400BadRequest,
            MatchNotReadyException => StatusCodes.Status400BadRequest,
            MatchNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }    
}
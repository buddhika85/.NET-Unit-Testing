using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.ErrorHandling;

public static class ErrorHandlingExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.Run(async context =>
        {
            ILogger logger = CreateLogger(context);

            Exception? exception = GetException(context);

            logger.LogError(
                exception,
                "Could not process a request on machine {Machine}. TraceId: {TraceId}",
                Environment.MachineName,
                Activity.Current?.TraceId);

            var problem = new ProblemDetails
            {
                Title = exception?.Message,
                Status = ErrorMapper.MapStatusCode(exception),
                Extensions =
                {
                    {"traceId", Activity.Current?.TraceId.ToString()}
                },
                Detail = GetProblemDetail(context, exception)
            };

            await Results.Problem(problem).ExecuteAsync(context);
        });
    }

    private static ILogger CreateLogger(HttpContext context)
    {
        return context.RequestServices.GetRequiredService<ILoggerFactory>()
                            .CreateLogger("Error Handling");
    }

    private static Exception? GetException(HttpContext context)
    {
        var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionDetails?.Error;
        return exception;
    }    

    private static string? GetProblemDetail(HttpContext context, Exception? exception)
    {
        var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();

        return environment.IsDevelopment()
            ? exception?.ToString()
            : null;
    }
}
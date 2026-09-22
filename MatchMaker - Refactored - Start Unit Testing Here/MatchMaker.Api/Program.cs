using Matchmaker.Api.Services;
using MatchMaker.Api.Data;
using MatchMaker.Api.ErrorHandling;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRepositories(builder.Configuration)
                .AddServices()
                .AddControllers();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
    exceptionHandlerApp.ConfigureExceptionHandler());
await app.Services.InitializeDbAsync();

app.MapControllers();

app.Run();
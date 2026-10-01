using MatchMaker.Api.Endpoints;
using MatchMaker.Api.ErrorHandling;
using MatchMaker.Application;
using MatchMaker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration)
                .AddApplication()
                .AddControllers();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp => 
    exceptionHandlerApp.ConfigureExceptionHandler());
await app.Services.InitializeDbAsync();

app.MapControllers();
app.MapMatchMakerEndpoints();

app.Run();
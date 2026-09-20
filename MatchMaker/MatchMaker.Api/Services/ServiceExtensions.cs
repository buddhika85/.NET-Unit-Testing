namespace Matchmaker.Api.Services;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IGameMatcher, GameMatcher>();
        return services;
    }
}

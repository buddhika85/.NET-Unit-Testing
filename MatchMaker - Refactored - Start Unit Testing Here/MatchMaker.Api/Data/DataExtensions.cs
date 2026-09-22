using MatchMaker.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MatchMaker.Api.Data;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MatchMakerDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static IServiceCollection AddRepositories(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connString = configuration.GetConnectionString("MatchMaker");
        services.AddSqlite<MatchMakerDbContext>(connString)
                .AddScoped<IGameMatchRepository, EntityFrameworkGameMatchRepository>();

        return services;
    }
}
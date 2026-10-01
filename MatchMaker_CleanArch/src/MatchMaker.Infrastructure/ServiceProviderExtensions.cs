using MatchMaker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MatchMaker.Infrastructure
{
    public static class ServiceProviderExtensions
    {
        public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MatchMakerDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
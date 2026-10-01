using MatchMaker.Application.Repositories;
using MatchMaker.Infrastructure.Data;
using MatchMaker.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MatchMaker.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("MatchMaker");
            services.AddSqlite<MatchMakerDbContext>(connString)
                    .AddScoped<IGameMatchRepository, EntityFrameworkGameMatchRepository>();

            return services;
        }
    }
}
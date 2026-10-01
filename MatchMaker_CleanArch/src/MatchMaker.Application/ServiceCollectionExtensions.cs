using Microsoft.Extensions.DependencyInjection;

namespace MatchMaker.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            });

            return services;
        }
    }
}
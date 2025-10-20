using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Mongo;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var settings = new MongoSettings();
        config.GetSection("Mongo").Bind(settings);

        services.AddSingleton(settings);
        services.AddSingleton<MongoDbContext>();

        // Aqui escolhemos usar Mongo para IMotoRepository no CP5
        services.AddScoped<IMotoRepository, MongoMotoRepository>();

        return services;
    }
}

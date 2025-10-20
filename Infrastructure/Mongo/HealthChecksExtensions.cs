using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.Mongo;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddAppAndMongoHealthChecks(this IServiceCollection services, MongoSettings mongo)
    {
        return services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy("API up"))
            .AddMongoDb(mongodbConnectionString: mongo.ConnectionString, name: "mongodb");
    }
}

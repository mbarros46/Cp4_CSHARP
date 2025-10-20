namespace Infrastructure.Mongo;

public sealed class MongoSettings
{
    public string ConnectionString { get; set; } = default!;
    public string Database { get; set; } = default!;
}

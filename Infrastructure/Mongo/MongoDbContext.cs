using MongoDB.Driver;
using Domain.Entities;

namespace Infrastructure.Mongo;

public sealed class MongoDbContext
{
    private readonly IMongoDatabase _db;

    public MongoDbContext(MongoSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _db = client.GetDatabase(settings.Database);

        var coll = _db.GetCollection<Moto>("motos");
        coll.Indexes.CreateOne(
            new CreateIndexModel<Moto>(Builders<Moto>.IndexKeys.Ascending(x => x.Placa))
        );
    }

    public IMongoCollection<Moto> Motos => _db.GetCollection<Moto>("motos");
}

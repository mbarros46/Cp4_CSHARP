using Domain.Entities;
using Domain.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Mongo;

public sealed class MongoMotoRepository : IMotoRepository
{
    private readonly MongoDbContext _ctx;

    public MongoMotoRepository(MongoDbContext ctx) => _ctx = ctx;

    public async Task<Moto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var cursor = await _ctx.Motos.FindAsync(x => x.Id == id, cancellationToken: ct);
        return await cursor.FirstOrDefaultAsync(ct);
    }

    public async Task<List<Moto>> ListAsync(CancellationToken ct = default)
    {
        var cursor = await _ctx.Motos.FindAsync(_ => true, cancellationToken: ct);
        return await cursor.ToListAsync(ct);
    }

    public async Task AddAsync(Moto entity, CancellationToken ct = default)
    {
        if (entity.Id == Guid.Empty)
        {
            var idProp = typeof(Moto).GetProperty("Id");
            idProp?.SetValue(entity, Guid.NewGuid());
        }
        await _ctx.Motos.InsertOneAsync(entity, cancellationToken: ct);
    }
    public async Task UpdateAsync(Moto entity)
    {
        // Use ReplaceOneAsync for async consistency. We intentionally do not throw if not matched
        await _ctx.Motos.ReplaceOneAsync(x => x.Id == entity.Id, entity);
    }

    public async Task RemoveAsync(Moto entity)
    {
        // Delete the document by id
        await _ctx.Motos.DeleteOneAsync(x => x.Id == entity.Id);
    }
}

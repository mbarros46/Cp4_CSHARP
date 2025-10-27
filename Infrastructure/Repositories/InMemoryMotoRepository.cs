using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories;

public class InMemoryMotoRepository : IMotoRepository
{
    private readonly ConcurrentDictionary<Guid, Moto> _store = new();

    public Task AddAsync(Moto entity, CancellationToken ct = default)
    {
        _store[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task<Moto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        _store.TryGetValue(id, out var moto);
        return Task.FromResult(moto);
    }

    public Task<List<Moto>> ListAsync(CancellationToken ct = default)
    {
        var list = _store.Values.ToList();
        return Task.FromResult(list);
    }

    public Task RemoveAsync(Moto entity)
    {
        _store.TryRemove(entity.Id, out _);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Moto entity)
    {
        _store[entity.Id] = entity;
        return Task.CompletedTask;
    }
}

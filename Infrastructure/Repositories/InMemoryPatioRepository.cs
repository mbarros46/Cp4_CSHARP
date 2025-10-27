using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories;

public class InMemoryPatioRepository : IPatioRepository
{
    private readonly ConcurrentDictionary<Guid, Patio> _store = new();

    public Task AddAsync(Patio entity, CancellationToken ct = default)
    {
        _store[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task<Patio?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        _store.TryGetValue(id, out var patio);
        return Task.FromResult(patio);
    }

    public Task<List<Patio>> ListAsync(CancellationToken ct = default)
    {
        var list = _store.Values.ToList();
        return Task.FromResult(list);
    }

    public void Remove(Patio entity) => _store.TryRemove(entity.Id, out _);

    public void Update(Patio entity) => _store[entity.Id] = entity;
}

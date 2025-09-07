using Domain.Entities;
namespace Domain.Repositories;
public interface IMotoRepository
{
    Task<Moto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Moto>> ListAsync(CancellationToken ct = default);
    Task AddAsync(Moto entity, CancellationToken ct = default);
    void Update(Moto entity);
    void Remove(Moto entity);
}

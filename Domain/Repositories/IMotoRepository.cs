using Domain.Entities;
namespace Domain.Repositories;
public interface IMotoRepository
{
    Task<Moto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Moto>> ListAsync(CancellationToken ct = default);
    Task AddAsync(Moto entity, CancellationToken ct = default);
    Task UpdateAsync(Moto entity);
    Task RemoveAsync(Moto entity);
}

using Domain.Entities;
namespace Domain.Repositories;
public interface IPatioRepository
{
    Task<Patio?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Patio>> ListAsync(CancellationToken ct = default);
    Task AddAsync(Patio entity, CancellationToken ct = default);
    void Update(Patio entity);
    void Remove(Patio entity);
}

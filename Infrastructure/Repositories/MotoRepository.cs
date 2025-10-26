using Domain.Entities;
using Domain.Repositories;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class MotoRepository : IMotoRepository
{
    private readonly ApplicationDbContext _ctx;
    public MotoRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public Task<List<Moto>> ListAsync(CancellationToken ct) =>
        _ctx.Motos.AsNoTracking().ToListAsync(ct);

    public Task<Moto?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _ctx.Motos.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task AddAsync(Moto entity, CancellationToken ct) =>
        _ctx.Motos.AddAsync(entity, ct).AsTask();

    public Task UpdateAsync(Moto entity)
    {
        _ctx.Motos.Update(entity);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Moto entity)
    {
        _ctx.Motos.Remove(entity);
        return Task.CompletedTask;
    }
}

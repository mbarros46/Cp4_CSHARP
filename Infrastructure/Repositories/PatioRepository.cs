using Domain.Entities;
using Domain.Repositories;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class PatioRepository : IPatioRepository
{
    private readonly ApplicationDbContext _ctx;
    public PatioRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public Task<List<Patio>> ListAsync(CancellationToken ct) =>
        _ctx.Patios.AsNoTracking().ToListAsync(ct);

    public Task<Patio?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _ctx.Patios.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task AddAsync(Patio entity, CancellationToken ct) =>
        _ctx.Patios.AddAsync(entity, ct).AsTask();

    public void Update(Patio entity) => _ctx.Patios.Update(entity);
    public void Remove(Patio entity) => _ctx.Patios.Remove(entity);
}

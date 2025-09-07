using Domain.Repositories;
using Infrastructure.EF;

namespace Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _ctx;
    public UnitOfWork(ApplicationDbContext ctx) => _ctx = ctx;
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);
}

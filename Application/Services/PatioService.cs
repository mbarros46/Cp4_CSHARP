using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;
public class PatioService
{
    private readonly IPatioRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public PatioService(IPatioRepository repo, IUnitOfWork uow, IMapper mapper)
    { _repo = repo; _uow = uow; _mapper = mapper; }

    public async Task<List<PatioResponse>> ListAsync(CancellationToken ct)
    {
        var list = await _repo.ListAsync(ct);
        return list.Select(_mapper.Map<PatioResponse>).ToList();
    }

    public async Task<PatioResponse> CreateAsync(PatioRequest dto, CancellationToken ct)
    {
        var entity = Patio.Create(dto.Nome, dto.Endereco, dto.Capacidade);
        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<PatioResponse>(entity);
    }

    public async Task<PatioResponse?> UpdateAsync(Guid id, PatioRequest dto, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity is null) return null;
        entity.AtualizarPatio(dto.Nome, dto.Endereco, dto.Capacidade);
        _repo.Update(entity);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<PatioResponse>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity is null) return false;
        _repo.Remove(entity);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}

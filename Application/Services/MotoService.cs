using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;
public class MotoService
{
    private readonly IMotoRepository _repo;
    private readonly IMapper _mapper;

    public MotoService(IMotoRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<List<MotoResponse>> ListAsync(CancellationToken ct)
    {
        var list = await _repo.ListAsync(ct);
        return list.Select(_mapper.Map<MotoResponse>).ToList();
    }

    public async Task<MotoResponse> CreateAsync(MotoRequest dto, CancellationToken ct)
    {
        var entity = new Moto(dto.Modelo, dto.Placa, dto.Status, dto.Ano, dto.PatioId);
    await _repo.AddAsync(entity, ct);
        return _mapper.Map<MotoResponse>(entity);
    }

    public async Task<MotoResponse?> UpdateAsync(Guid id, MotoRequest dto, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity is null) return null;
        entity.AtualizarDados(dto.Modelo, dto.Placa, dto.Status, dto.Ano, dto.PatioId);
    _repo.Update(entity);
        return _mapper.Map<MotoResponse>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity is null) return false;
    _repo.Remove(entity);
        return true;
    }
}

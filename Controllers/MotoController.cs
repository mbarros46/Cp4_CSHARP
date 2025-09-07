using c_.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;


namespace MottuCrudAPI.Controllers
{
    [ApiController]
    [Route("api/motos")]
    [Tags("Motos")]
    public class MotoController : ControllerBase
    {
        private readonly MotoService _svc;
        public MotoController(MotoService svc) => _svc = svc;

        [HttpGet]
        public async Task<ActionResult<List<MotoResponse>>> Get(CancellationToken ct)
            => Ok(await _svc.ListAsync(ct));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MotoResponse>> Get(Guid id, CancellationToken ct)
        {
            var list = await _svc.ListAsync(ct);
            var moto = list.FirstOrDefault(x => x.Id == id);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        [HttpPost]
        public async Task<ActionResult<MotoResponse>> Post([FromBody] MotoRequest dto, CancellationToken ct)
        {
            var created = await _svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<MotoResponse>> Put(Guid id, [FromBody] MotoRequest dto, CancellationToken ct)
        {
            var updated = await _svc.UpdateAsync(id, dto, ct);
            if (updated is null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var ok = await _svc.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services;

namespace MottuCrudAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/patios")]
    [Tags("Pátios")]
    public class PatioController : ControllerBase
    {
        private readonly PatioService _svc;
        public PatioController(PatioService svc) => _svc = svc;

        [HttpGet]
        public async Task<ActionResult<List<PatioResponse>>> Get(CancellationToken ct)
            => Ok(await _svc.ListAsync(ct));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PatioResponse>> Get(Guid id, CancellationToken ct)
        {
            var list = await _svc.ListAsync(ct);
            var patio = list.FirstOrDefault(x => x.Id == id);
            if (patio == null) return NotFound();
            return Ok(patio);
        }

        [HttpPost]
        public async Task<ActionResult<PatioResponse>> Post([FromBody] PatioRequest dto, CancellationToken ct)
        {
            var created = await _svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<PatioResponse>> Put(Guid id, [FromBody] PatioRequest dto, CancellationToken ct)
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

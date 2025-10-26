using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services;

namespace MottuCrudAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/patios")]
    [Tags("Pátios")]
    public class PatioController : ControllerBase
    {
        private readonly PatioService _svc;
        public PatioController(PatioService svc) => _svc = svc;

        /// <summary>
        /// Lista todos os pátios cadastrados.
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        [HttpGet]
        [ProducesResponseType(typeof(List<PatioResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PatioResponse>>> Get(CancellationToken ct)
            => Ok(await _svc.ListAsync(ct));

        /// <summary>
        /// Obtém um pátio pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do pátio</param>
        /// <param name="ct">Token de cancelamento</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PatioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatioResponse>> Get(Guid id, CancellationToken ct)
        {
            var list = await _svc.ListAsync(ct);
            var patio = list.FirstOrDefault(x => x.Id == id);
            if (patio == null) return NotFound();
            return Ok(patio);
        }

        /// <summary>
        /// Cria um novo pátio.
        /// </summary>
        /// <param name="dto">Dados do pátio a ser criado</param>
        /// <param name="ct">Token de cancelamento</param>
        [HttpPost]
        [ProducesResponseType(typeof(PatioResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PatioResponse>> Post([FromBody] PatioRequest dto, CancellationToken ct)
        {
            var created = await _svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Atualiza os dados de um pátio existente.
        /// </summary>
        /// <param name="id">Identificador do pátio</param>
        /// <param name="dto">Dados atualizados</param>
        /// <param name="ct">Token de cancelamento</param>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(PatioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatioResponse>> Put(Guid id, [FromBody] PatioRequest dto, CancellationToken ct)
        {
            var updated = await _svc.UpdateAsync(id, dto, ct);
            if (updated is null) return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Remove um pátio pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do pátio</param>
        /// <param name="ct">Token de cancelamento</param>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var ok = await _svc.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services;


namespace MottuCrudAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/motos")]
    [Tags("Motos")]
    public class MotoController : ControllerBase
    {
        private readonly MotoService _svc;
        public MotoController(MotoService svc) => _svc = svc;

        /// <summary>
        /// Lista todas as motos cadastradas.
        /// </summary>
        /// <remarks>
        /// Retorna uma lista com as motos existentes no sistema.
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(List<MotoResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MotoResponse>>> Get(CancellationToken ct)
            => Ok(await _svc.ListAsync(ct));

    /// <summary>
    /// Obtém uma moto pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador da moto (GUID)</param>
    /// <param name="ct">Token de cancelamento</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MotoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MotoResponse>> Get(Guid id, CancellationToken ct)
        {
            var list = await _svc.ListAsync(ct);
            var moto = list.FirstOrDefault(x => x.Id == id);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

    /// <summary>
    /// Cria uma nova moto.
    /// </summary>
    /// <param name="dto">Dados da moto a ser criada</param>
    /// <param name="ct">Token de cancelamento</param>
        [HttpPost]
        [ProducesResponseType(typeof(MotoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MotoResponse>> Post([FromBody] MotoRequest dto, CancellationToken ct)
        {
            var created = await _svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

    /// <summary>
    /// Atualiza os dados de uma moto existente.
    /// </summary>
    /// <param name="id">Identificador da moto</param>
    /// <param name="dto">Dados atualizados</param>
    /// <param name="ct">Token de cancelamento</param>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(MotoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MotoResponse>> Put(Guid id, [FromBody] MotoRequest dto, CancellationToken ct)
        {
            var updated = await _svc.UpdateAsync(id, dto, ct);
            if (updated is null) return NotFound();
            return Ok(updated);
        }

    /// <summary>
    /// Remove uma moto pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da moto</param>
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

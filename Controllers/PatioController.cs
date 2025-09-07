using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using MottuCrudAPI.Persistense;
using MottuCrudAPI.Infrastructure;
using MottuCrudAPI.DTO.Response;
using MottuCrudAPI.DTO.Request;


namespace MottuCrudAPI.Controllers
{
    [Route("api/[controller]")]
    [Tags("Pátios")]
    [ApiController]
    public class PatioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatioController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os pátios, com filtros opcionais por nome e endereço
        /// </summary>
        /// <param name="nome">Filtro opcional pelo nome do pátio</param>
        /// <param name="endereco">Filtro opcional pelo endereço</param>
        /// <returns>Lista de pátios</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PatioResponse>>> GetPatios([FromQuery] string? nome, [FromQuery] string? endereco)
        {
            var query = _context.Patios.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(p => p.Nome.Contains(nome));

            if (!string.IsNullOrEmpty(endereco))
                query = query.Where(p => p.Endereco.Contains(endereco));

            var patios = await query.ToListAsync();

            var response = patios.Select(p => new PatioResponse
            {
                Id = p.Id,
                Nome = p.Nome,
                Endereco = p.Endereco,
                Capacidade = p.Capacidade,
                OcupacaoAtual = p.OcupacaoAtual
            });

            return Ok(response);
        }


        /// <summary>
        /// Obtém um pátio pelo ID
        /// </summary>
        /// <param name="id">ID do pátio</param>
        /// <returns>Pátio encontrado ou NotFound</returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Patio>> GetPatio(Guid id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
                return NotFound();

            return Ok(patio);
        }

        /// <summary>
        /// Cria um novo pátio
        /// </summary>
        /// <param name="patio">Dados do pátio</param>
        /// <returns>Pátio criado</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PatioResponse>> CreatePatio([FromBody] PatioRequest patioRequest)
        {
            if (patioRequest == null)
                return BadRequest();

            var patio = Patio.Create(patioRequest.Nome, patioRequest.Endereco, patioRequest.Capacidade);

            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();

            var patioResponse = new PatioResponse
            {
                Id = patio.Id,
                Nome = patio.Nome,
                Endereco = patio.Endereco,
                Capacidade = patio.Capacidade
            };

            return CreatedAtAction(nameof(GetPatio), new { id = patio.Id }, patioResponse);
        }


        /// <summary>
        /// Atualiza um pátio existente
        /// </summary>
        /// <param name="id">ID do pátio a atualizar</param>
        /// <param name="patioRequest">Dados atualizados do pátio</param>
        /// <returns>Pátio atualizado ou NotFound</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Patio>> UpdatePatio(Guid id, [FromBody] PatioRequest patioRequest)
        {
            var patioExistente = await _context.Patios.FindAsync(id);
            if (patioExistente == null)
                return NotFound();

            try
            {
                patioExistente.AtualizarPatio(patioRequest.Nome, patioRequest.Endereco, patioRequest.Capacidade);
                await _context.SaveChangesAsync();
                return Ok(patioExistente);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
        }



        /// <summary>
        /// Remove um pátio pelo ID
        /// </summary>
        /// <param name="id">ID do pátio</param>
        /// <returns>NoContent ou mensagem de erro</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeletePatio(Guid id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
                return NotFound();

            
            {
                // Buscar motos associadas 
                var motosAssociadas = await _context.Motos
                    .Where(m => m.PatioId != null)
                    .ToListAsync();

                bool existeMotoAssociada = motosAssociadas.Any(m => m.PatioId == id);

                if (existeMotoAssociada)
                {
                    return BadRequest(new
                    {
                        message = "Não é possível deletar o pátio: existem motos associadas a ele."
                    });
                }

                // removendo o pátio

                _context.Patios.Remove(patio);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            
        }




    }
}

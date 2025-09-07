using c_.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuCrudAPI.DTO.Request;
using MottuCrudAPI.DTO.Response;
using MottuCrudAPI.Infrastructure;
using MottuCrudAPI.Persistense;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;


namespace MottuCrudAPI.Controllers
{
   
    [Route("api/[controller]")]
    [Tags("Motos")]
    [ApiController]
    public class MotoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MotoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna uma lista de motos
        /// </summary>
        /// <remarks>
        /// Exemplo de solicitação:
        /// 
        ///     GET api/moto
        /// 
        /// </remarks>
        /// <response code="200">Retorna a lista de motos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotos()
        {
            return await _context.Motos.ToListAsync();
        }

        /// <summary>
        /// Obtém uma moto específica pelo ID
        /// </summary>
        /// <param name="id">ID da moto</param>
        /// <returns>Moto ou NotFound</returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<MotoResponse>> GetMoto(Guid id)
        {
            var moto = await _context.Motos.FindAsync(id);

            if (moto == null)
                return NotFound();

            var motoDto = new MotoResponse
            {
                Id = moto.Id,
                Modelo = moto.Modelo,
                Placa = moto.Placa,
                Status = moto.Status,
                Ano = moto.Ano,
                PatioId = moto.PatioId
            };


            return Ok(motoDto);
        }

        /// <summary>
        /// Cria uma nova moto
        /// </summary>
        /// <param name="motoRequest">Dados para criar a moto</param>
        /// <returns>Moto criada</returns>
        [HttpPost]
        public async Task<ActionResult<Moto>> PostMoto(MotoRequest motoRequest)
        {
            if (motoRequest == null)
                return BadRequest("Dados da moto não informados.");

            // Extraindo os dados do DTO
            var modelo = motoRequest.Modelo;
            var placa = motoRequest.Placa;
            var status = motoRequest.Status;
            var ano = motoRequest.Ano;
            var patioId = motoRequest.PatioId;

            // Criando a entidade usando o método estático Create
            var moto = Moto.Create(modelo, placa, status, ano, patioId);

            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMoto), new { id = moto.Id }, moto);
        }


        /// <summary>
        /// Atualiza uma moto existente
        /// </summary>
        /// <param name="id">ID da moto a atualizar</param>
        /// <param name="motoRequest">Dados atualizados da moto</param>
        /// <returns>Moto atualizada ou NotFound</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<MotoResponse>> UpdateMoto(Guid id, MotoRequest motoRequest)
        {
            if (motoRequest == null)
                return BadRequest("Dados da moto não informados.");

            var motoExistente = await _context.Motos.FindAsync(id);
            if (motoExistente == null)
                return NotFound();

            try
            {
                motoExistente.AtualizarDados(
                    motoRequest.Modelo,
                    motoRequest.Placa,
                    motoRequest.Status,
                    motoRequest.Ano,
                    motoRequest.PatioId);

                await _context.SaveChangesAsync();

                var motoDto = new MotoResponse
                {
                    Id = motoExistente.Id,
                    Modelo = motoExistente.Modelo,
                    Placa = motoExistente.Placa,
                    Status = motoExistente.Status,
                    Ano = motoExistente.Ano,
                    PatioId = motoExistente.PatioId
                };

                return Ok(motoDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }



        /// <summary>
        /// Remove uma moto pelo ID
        /// </summary>
        /// <param name="id">ID da moto</param>
        /// <returns>NoContent ou NotFound</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoto(Guid id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                return NotFound();

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

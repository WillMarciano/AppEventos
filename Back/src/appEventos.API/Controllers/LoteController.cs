using AppEventos.Application;
using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoteController : ControllerBase
    {
        private const string errorResponse = "Erro ao ao tentar * lote(s)";
        private readonly ILoteService _loteService;

        public LoteController(ILoteService loteService) => _loteService = loteService;

        /// <summary>
        /// Buscar lotes pelo EventoId
        /// </summary>
        /// <param name="eventoId"></param>
        /// <returns></returns>
        [HttpGet("{eventoId}")]
        public async Task<IActionResult> GetLotesByEventoId(int eventoId)
        {
            try
            {
                var lotes = await _loteService.GetLotesByEventoIdAsync(eventoId);
                return lotes == null ? NoContent() : Ok(lotes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
            }
        }

        /// <summary>
        /// Salvar Lotes
        /// </summary>
        /// <param name="eventoId"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var evento = await _loteService.SaveLotesAsync(eventoId, models);
                return evento == null ? NoContent() : Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "salvar")}: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletar Lotes
        /// </summary>
        /// <param name="eventoId"></param>
        /// <param name="loteId"></param>
        /// <returns></returns>
        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteService.GetLotesByIdAsync(eventoId, loteId);
                if (lote == null) return NoContent();

                return await _loteService.DeleteLoteAsync(lote.EventoId, lote.Id) ?
                    Ok(new { message = "Lote Deletado" }) :
                    BadRequest(errorResponse.Replace("*", "deletar"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "deletar")} {ex.Message}");
            }
        }
    }
}

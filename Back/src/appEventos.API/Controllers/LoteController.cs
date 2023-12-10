using appEventos.Application;
using appEventos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace appEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoteController : ControllerBase
    {
        private const string errorResponse = "Erro ao ao tentar * evento";
        private readonly ILoteService _loteService;

        public LoteController(ILoteService loteService) => _loteService = loteService;

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> GetByTema(int eventoId)
        {
            try
            {
                var lotes = await _loteService.GetLotesByEventoId(eventoId);
                return lotes == null ? NoContent() : Ok(lotes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
            }
        }
    }
}

using AppEventos.API.Extensions;
using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using AppEventos.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RedesSociaisController : ControllerBase
    {
        private const string errorResponse = "Erro ao ao tentar * Redes Sociais";
        private readonly IRedeSocialService _redeSocialService;
        private readonly IEventoService _eventoService;
        private readonly IPalestranteService _palestranteService;

        public RedesSociaisController(IRedeSocialService redeSocialService,
                                      IEventoService eventoService,
                                      IPalestranteService palestranteService)
        {
            _redeSocialService = redeSocialService;
            _eventoService = eventoService;
            _palestranteService = palestranteService;
        }
        /// <summary>
        /// Retorna todas redes Sociais por Evento ou Palestrante
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isEvento"></param>
        /// <returns></returns>
        [HttpGet("GetAllRedesSociaisById/{id}/{isEvento}")]
        public async Task<IActionResult> GetAllRedesSociaisById(int id, bool isEvento)
        {
            try
            {
                if (!(await AutorRedeSocial(id, isEvento))) return Unauthorized();

                var redes = await _redeSocialService.GetAllEventosAsync(id, isEvento);
                return redes == null || redes.Length == 0 ? NoContent() : Ok(redes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
            }
        }

        [NonAction]
        private async Task<bool> AutorRedeSocial(int id, bool isEvento)
        {
            var userId = User.GetUserId();

            if (isEvento)
            {
                var evento = await _eventoService.GetEventoByIdAsync(userId, id);
                if (evento == null) return false;
            }
            else
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(userId);
                if (palestrante == null) return false;
            }

            return true;
        }

    }
}

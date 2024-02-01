using AppEventos.API.Extensions;
using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using AppEventos.Domain.Models;
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
        [HttpGet("GetAll/{id}")]
        public async Task<IActionResult> GetAll(int id, bool isEvento)
        {
            try
            {
                if (!(await AutorRedeSocial(id, isEvento))) return Unauthorized();
                id = !isEvento ? (int)_palestranteService.GetPalestranteByUserIdAsync(User.GetUserId()).Result?.Id! : 0;

                var redes = await _redeSocialService.GetAllRedesSociaisAsync(id, isEvento);
                return redes == null || redes.Length == 0 ? NoContent() : Ok(redes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
            }
        }

        /// <summary>
        /// Salva rede social por Evento ou Palestrante
        /// </summary>
        /// <param name="id"></param>
        /// <param name="models"></param>
        /// <param name="isEvento"></param>
        /// <returns></returns>
        [HttpPut("Salvar/{id}")]
        public async Task<IActionResult> Salvar(int id, RedeSocialDto[] models, bool isEvento)
        {
            try
            {
                if (!(await AutorRedeSocial(id, isEvento))) return Unauthorized();

                var palestranteId = !isEvento ? _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId()).Result?.Id : null;
                    

                var redeSocial = await _redeSocialService.SaveAsync(id, palestranteId, models, isEvento);
                if (redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{errorResponse.Replace("*", "salvar")}: {ex.Message}");
            }
        }

        /// <summary>
        /// Deleta rede social por Evento
        /// </summary>
        /// <param name="eventoId"></param>
        /// <param name="redeSocialId"></param>
        /// <returns></returns>
        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                if (!(await AutorRedeSocial(eventoId, true))) return Unauthorized();

                var RedeSocial = await _redeSocialService.GetRedeSocialByIdAsync(eventoId, redeSocialId, true);
                if (RedeSocial == null) return NoContent();

                return await _redeSocialService.DeleteAsync(eventoId, redeSocialId, true)
                       ? Ok(new { message = "Rede Social Deletada" })
                       : throw new Exception("Ocorreu um problem não específico ao tentar deletar Rede Social por Evento.");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "deletar");
            }
        }


        /// <summary>
        /// Deleta rede social por Palestrante
        /// </summary>
        /// <param name="redeSocialId"></param>
        /// <returns></returns>
        [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if (palestrante == null) return Unauthorized();

                var RedeSocial = await _redeSocialService.GetRedeSocialByIdAsync(palestrante.Id, redeSocialId, false);
                if (RedeSocial == null) return NoContent();

                return await _redeSocialService.DeleteAsync(palestrante.Id, redeSocialId, false)
                       ? Ok(new { message = "Rede Social Deletada" })
                       : throw new Exception("Ocorreu um problem não específico ao tentar deletar Rede Social por Palestrante.");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "deletar");
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

        [NonAction]
        private IActionResult HandleException(Exception ex, string action)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"{errorResponse.Replace("*", action)}: {ex.Message}");
        }

    }
}

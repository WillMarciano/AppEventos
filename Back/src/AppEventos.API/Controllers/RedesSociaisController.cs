﻿using AppEventos.API.Extensions;
using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
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
        /// Retorna todas redes Sociais por Evento
        /// </summary>
        /// <param name="eventoId"></param>
        /// <returns></returns>
        [HttpGet("GetAllByEventoId/{eventoId}")]
        public async Task<IActionResult> GetAllByEventoId(int eventoId)
        {
            try
            {
                if (!(await AutorRedeSocial(eventoId, true))) return Unauthorized();

                var redes = await _redeSocialService.GetAllEventosAsync(eventoId, true);
                return redes == null || redes.Length == 0 ? NoContent() : Ok(redes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
            }
        }

        /// <summary>
        /// Retorna todas redes Sociais por Palestrante
        /// </summary>
        /// <param name="palestranteId"></param>
        /// <returns></returns>
        [HttpGet("GetAllByPalestranteId/{palestranteId}")]
        public async Task<IActionResult> GetAllByPalestranteId(int palestranteId)
        {
            try
            {
                if (!(await AutorRedeSocial(palestranteId, false))) return Unauthorized();

                var redeSocial = await _redeSocialService.GetAllEventosAsync(palestranteId, false);
                if (redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
            }
        }

        /// <summary>
        /// Salva rede social por Evento
        /// </summary>
        /// <param name="eventoId"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                if (!(await AutorRedeSocial(eventoId, true))) return Unauthorized();

                var redeSocial = await _redeSocialService.SaveAsync(eventoId, models, true);
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
        /// Salva rede social por Palestrante
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] models)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if (palestrante == null) return Unauthorized();

                var redeSocial = await _redeSocialService.SaveAsync(palestrante.Id, models, false);
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{errorResponse.Replace("*", "deletar")}: {ex.Message}");
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{errorResponse.Replace("*", "deletar")}: {ex.Message}");
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

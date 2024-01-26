using AppEventos.API.Extensions;
using AppEventos.API.Helpers;
using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using AppEventos.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppEventos.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private const string errorResponse = "Erro ao ao tentar * evento";
    private readonly IUtil _util;
    private readonly IEventoService _eventoService;
    private readonly IAccountService _accountService;
    private readonly string _destino = "Images";

    public EventosController(IEventoService eventoService,
                             IAccountService accountService,
                             IUtil util)
    {
        _eventoService = eventoService;
        _accountService = accountService;
        _util = util;
    }

    /// <summary>
    /// Retona Todos Eventos
    /// </summary>
    /// <returns></returns>
    [HttpGet]

    public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
    {
        try
        {
            var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), pageParams);

            //return eventos == null ? NoContent() : Ok(eventos);
            if (eventos == null) return NoContent();

            Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);
            return Ok(eventos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
        }
    }

    /// <summary>
    /// Retorna Evento por Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id);
            return evento == null ? NoContent() : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
        }
    }

    /// <summary>
    /// Salva Evento
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post(EventoDto model)
    {
        try
        {
            var evento = await _eventoService.AddEventos(User.GetUserId(), model);
            return evento == null ? NoContent() : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                $"{errorResponse.Replace("*", "adicionar")} {ex.Message}");
        }
    }

    /// <summary>
    /// Salva Imagem do Evento
    /// </summary>
    /// <param name="eventoId"></param>
    /// <returns></returns>
    [HttpPost("upload-image/{eventoId}")]
    public async Task<IActionResult> UploadImage(int eventoId)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId);
            if (evento == null) return NoContent();

            var file = Request.Form.Files[0];
            if (file.Length > 0 && evento.ImagemUrl != null)
            {
                _util.DeleteImage(evento.ImagemUrl, _destino);
                evento.ImagemUrl = await _util.SaveImage(file, _destino);
            }
            var eventoRetorno = await _eventoService.UpdateEvento(User.GetUserId(), eventoId, evento);
            return eventoRetorno == null ? NoContent() : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                $"{errorResponse.Replace("*", "Upload de foto do")} {ex.Message}");
        }
    }

    /// <summary>
    /// Atualiza Evento
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, EventoDto model)
    {
        try
        {
            var evento = await _eventoService.UpdateEvento(User.GetUserId(), id, model);
            return evento == null ? NoContent() : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                $"{errorResponse.Replace("*", "atualizar")}: {ex.Message}");
        }
    }

    /// <summary>
    /// Deleta Evento
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id);
            if (evento == null) return NoContent();

            if (await _eventoService.DeleteEvento(User.GetUserId(), id))
            {
                if (evento.ImagemUrl != null)
                    _util.DeleteImage(evento.ImagemUrl, _destino);
                return Ok(new { message = "Deletado" });
            }
            else
                return BadRequest(errorResponse.Replace("*", "deletar"));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                $"{errorResponse.Replace("*", "deletar")} {ex.Message}");
        }
    }
}

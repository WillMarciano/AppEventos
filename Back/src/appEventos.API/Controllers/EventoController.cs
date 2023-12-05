using appEventos.Application.Dtos;
using appEventos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace appEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private const string errorResponse = "Erro ao ao tentar * evento";
    private readonly IEventoService _eventoService;

    public EventosController(IEventoService eventoService) => _eventoService = eventoService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var eventos = await _eventoService.GetAllEventosAsync();

            return eventos == null ? NoContent() : Ok(eventos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(id);
            return evento == null ? NoContent() : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
        }
    }

    [HttpGet("tema/{tema}")]
    public async Task<IActionResult> GetByTema(string tema)
    {
        try
        {
            var evento = await _eventoService.GetAllEventosByTemaAsync(tema);
            return evento == null ? NoContent() : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(EventoDto model)
    {
        try
        {
            var evento = await _eventoService.AddEventos(model);
            return evento == null ? NoContent() : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "adicionar")} {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, EventoDto model)
    {
        try
        {
            var evento = await _eventoService.UpdateEvento(id, model);
            return evento == null ? NoContent() : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "atualizar")}: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(id);
            if (evento == null) return NoContent();

            return await _eventoService.DeleteEvento(id) ?
                Ok("Evento Deletado") :
                BadRequest(errorResponse.Replace("*", "deletar"));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "deletar")} {ex.Message}");
        }
    }
}

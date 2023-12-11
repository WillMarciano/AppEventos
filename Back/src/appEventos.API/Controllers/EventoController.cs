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

    /// <summary>
    /// Retona Todos Eventos
    /// </summary>
    /// <returns></returns>
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
            var evento = await _eventoService.GetEventoByIdAsync(id);
            return evento == null ? NoContent() : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
        }
    }

    /// <summary>
    /// Retorna Evento por Tema
    /// </summary>
    /// <param name="tema"></param>
    /// <returns></returns>
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
            var evento = await _eventoService.AddEventos(model);
            return evento == null ? NoContent() : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "adicionar")} {ex.Message}");
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
            var evento = await _eventoService.UpdateEvento(id, model);
            return evento == null ? NoContent() : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "atualizar")}: {ex.Message}");
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
            var evento = await _eventoService.GetEventoByIdAsync(id);
            if (evento == null) return NoContent();

            return await _eventoService.DeleteEvento(id) ?
                Ok(new { message = "Deletado" }) :
                BadRequest(errorResponse.Replace("*", "deletar"));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "deletar")} {ex.Message}");
        }
    }
}

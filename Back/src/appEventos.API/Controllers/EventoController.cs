using appEventos.Application.Dtos;
using appEventos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace appEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private const string notFoundString = "Nenhum Evento Encontrado";
    private const string errorResponse = "Erro ao ao tentar * evento";
    private readonly IEventoService _eventoService;

    public EventosController(IEventoService eventoService) => _eventoService = eventoService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var eventos = await _eventoService.GetAllEventosAsync();

            return eventos.Any() ? 
                Ok(eventos) : 
                NotFound(notFoundString);
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
            return evento != null ? 
                Ok(evento) : 
                NotFound($"{notFoundString}");
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
            return evento.Any() ? 
                Ok(evento) : 
                NotFound($"{notFoundString}");
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
            return evento != null ? 
                Ok(evento) : 
                BadRequest(errorResponse.Replace("*", "adicionar"));
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
            return evento != null ? 
                Ok(evento) : 
                BadRequest(errorResponse.Replace("*", "atualizar"));
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

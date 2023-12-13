using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private const string errorResponse = "Erro ao ao tentar * evento";
    private readonly IEventoService _eventoService;
    private readonly IWebHostEnvironment _hostEnvironment;
    public EventosController(IEventoService eventoService, IWebHostEnvironment hostEnvironment)
    {
        _eventoService = eventoService;
        _hostEnvironment = hostEnvironment;
    }

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

    [HttpPost("upload-image/{eventoId")]
    public async Task<IActionResult> UploadImage(int eventoId)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(eventoId);
            if (evento == null) return NoContent();
            var file = Request.Form.Files[0];
            if (file.Length > 0 && evento.ImagemUrl != null)
            {
                DeleteImage(evento.ImagemUrl);
                //evento.ImagemUrl = SaveImage(file);
            }
            var EventoRetorno = await _eventoService.UpdateEvento(eventoId, evento);
            return EventoRetorno == null ? NoContent() : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "adicionar")} {ex.Message}");
        }
    }

    [NonAction]
    private void DeleteImage(string imageName)
    {
        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);
        if (System.IO.File.Exists(imagePath))
            System.IO.File.Delete(imagePath);

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

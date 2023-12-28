using AppEventos.API.Extensions;
using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppEventos.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private const string errorResponse = "Erro ao ao tentar * evento";
    private string pathFile = "";
    private readonly IEventoService _eventoService;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IAccountService _accountService;
    public EventosController(IEventoService eventoService, 
                             IWebHostEnvironment hostEnvironment, 
                             IAccountService accountService)
    {
        _eventoService = eventoService;
        _hostEnvironment = hostEnvironment;
        pathFile = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images");
        _accountService = accountService;
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
            var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId());

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
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id);
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
            var evento = await _eventoService.GetAllEventosByTemaAsync(User.GetUserId(), tema);
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
            var evento = await _eventoService.AddEventos(User.GetUserId(), model);
            return evento == null ? NoContent() : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "adicionar")} {ex.Message}");
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
                DeleteImage(evento.ImagemUrl);
                evento.ImagemUrl = await SaveImage(file);
            }
            var eventoRetorno = await _eventoService.UpdateEvento(User.GetUserId(), eventoId, evento);
            return eventoRetorno == null ? NoContent() : Ok(evento);
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

    [NonAction]
    private async Task<string> SaveImage(IFormFile imageFile)
    {
        var imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                    .Take(10)
                                    .ToArray())
                                    .Replace(' ', '-');

        imageName = $"{imageName}{DateTime.UtcNow.ToString("yymssfff")}{Path.GetExtension(imageFile.FileName)}";
        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);

        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }
        return imageName;

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
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id);
            if (evento == null) return NoContent();

            if (await _eventoService.DeleteEvento(User.GetUserId(), id))
            {
                if (evento.ImagemUrl != null)
                    DeleteImage(evento.ImagemUrl);
                return Ok(new { message = "Deletado" });
            }
            else
                return BadRequest(errorResponse.Replace("*", "deletar"));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "deletar")} {ex.Message}");
        }
    }
}

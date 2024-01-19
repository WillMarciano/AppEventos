using AppEventos.API.Extensions;
using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using AppEventos.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppEventos.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PalestrantesController : ControllerBase
{
    private const string errorResponse = "Erro ao ao tentar * palestrante";
    private string pathFile = "";
    private readonly IPalestranteService _palestranteService;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IAccountService _accountService;
    public PalestrantesController(IPalestranteService palestranteService, 
                             IWebHostEnvironment hostEnvironment, 
                             IAccountService accountService)
    {
        _palestranteService = palestranteService;
        _hostEnvironment = hostEnvironment;
        pathFile = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images");
        _accountService = accountService;
    }

    /// <summary>
    /// Retona Todos os Palestrantes
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetAll")]

    public async Task<IActionResult> GetAll([FromQuery]PageParams pageParams)
    {
        try
        {
            var palestrantes = await _palestranteService.GetAllPalestrantesAsync(pageParams, true);

            if(palestrantes == null || palestrantes.Count == 0) return NoContent();

            Response.AddPagination(palestrantes.CurrentPage, 
                                   palestrantes.PageSize, 
                                   palestrantes.TotalCount, 
                                   palestrantes.TotalPages);

            return Ok(palestrantes);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
        }
    }

    /// <summary>
    /// Retorna Palestrante
    /// </summary>
    /// <returns>
    /// </returns>
    [HttpGet("GetPalestrantes")]
    public async Task<IActionResult> GetPalestrantes()
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
            return palestrante == null ? NoContent() : Ok(palestrante);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
        }
    }

    /// <summary>
    /// Salva Palestrante
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post(PalestranteAddDto model)
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
            if (palestrante == null)
                palestrante = await _palestranteService.AddPalestranteAsync(User.GetUserId(), model);

            return Ok(palestrante);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                $"{errorResponse.Replace("*", "adicionar")} {ex.Message}");
        }
    }

    /// <summary>
    /// Atualiza Palestrante
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(PalestranteUpdateDto model)
    {
        try
        {
            var palestrante = await _palestranteService.UpdatePalestranteAsync(User.GetUserId(), model);
            return palestrante == null ? NoContent() : Ok(palestrante);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                $"{errorResponse.Replace("*", "atualizar")}: {ex.Message}");
        }
    }
}

using appEventos.Domain.Models;
using appEventos.Repository.Context;
using Microsoft.AspNetCore.Mvc;
namespace appEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly AppEventosContext _context;
    public EventosController(AppEventosContext context) => _context = context;

    [HttpGet]
    public IEnumerable<Evento> Get() => _context.Eventos;

    [HttpGet("{id}")]
    public Evento? GetBydId(int id) => _context.Eventos.FirstOrDefault(evento => evento.Id == id);
}

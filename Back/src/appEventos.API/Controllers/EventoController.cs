using appEventos.API.Data;
using appEventos.API.Models;
using Microsoft.AspNetCore.Mvc;
namespace appEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly DataContext _context;
    public EventosController(DataContext context) => _context = context;

    [HttpGet]
    public IEnumerable<Evento> Get() => _context.Eventos;

    [HttpGet("{id}")]
    public Evento? GetBydId(int id) => _context.Eventos.FirstOrDefault(evento => evento.EventoId == id);
}

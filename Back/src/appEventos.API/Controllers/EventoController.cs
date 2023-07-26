using appEventos.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace appEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{
    public IEnumerable<Evento> _evento = new Evento[]
    {
        new Evento()
        {
            EventoId = 1,
            Local = "Online",
            DataEvento = DateTime.Today.ToString(),
            Tema = "Angualar e .Net Core",
            Lote = "1º Lote",
            QtdPessoas = 200
        },
        new Evento
        {
            EventoId = 2,
            Local = "Online",
            DataEvento = DateTime.Today.AddDays(2).ToString(),
            Tema = "Angualar e .Net Core 5",
            Lote = "2º Lote",
            QtdPessoas = 250
        }
    };

    public EventoController() { }

    [HttpGet]
    public IEnumerable<Evento> Get() => _evento;

    [HttpGet("{id}")]
    public IEnumerable<Evento> GetBydId(int id) => _evento.Where(evento => evento.EventoId == id);
}

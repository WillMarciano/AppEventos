using Microsoft.AspNetCore.Mvc;

namespace appEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{
    [HttpGet(Name = "GetWeatherForecast")]
    public string Get() => "value";
}

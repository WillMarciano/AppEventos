using AppEventos.Application.Dtos;
using AppEventos.Domain.Models;
using AppEventos.Repository.Models;

namespace AppEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDto?> AddEventos(int userId, EventoDto model);
        Task<EventoDto?> UpdateEvento(int userId, int eventoId, EventoDto model);
        Task<bool> DeleteEvento(int userId, int eventoId);
        Task<PageList<EventoDto>?> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);
        Task<EventoDto?> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
    }
}

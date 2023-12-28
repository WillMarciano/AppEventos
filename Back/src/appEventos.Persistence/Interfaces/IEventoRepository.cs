using AppEventos.Domain.Models;

namespace AppEventos.Repository.Interfaces
{
    public interface IEventoRepository
    {
        Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);
        Task<Evento?> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
    }
}
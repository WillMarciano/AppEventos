using AppEventos.Domain.Models;
using AppEventos.Repository.Models;

namespace AppEventos.Repository.Interfaces
{
    public interface IEventoRepository
    {
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);        
        Task<Evento?> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
    }
}
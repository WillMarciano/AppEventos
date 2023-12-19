using AppEventos.Domain.Models;

namespace AppEventos.Repository.Interfaces
{
    public interface ILoteRepository
    {
        Task<Lote[]?> GetLotesByEventoIdAsync(int eventoId);
        Task<Lote?> GetLoteByIdAsync(int eventoId, int id);
    }
}

using AppEventos.Domain.Models;

namespace AppEventos.Repository.Interfaces
{
    public interface ILoteRepository
    {
        Task<Lote[]?> GetLotesByEventoId(int eventoId);
        Task<Lote?> GetLoteById(int eventoId, int id);
    }
}

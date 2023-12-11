using appEventos.Domain.Models;

namespace appEventos.Repository.Interfaces
{
    public interface ILoteRepository
    {
        Task<Lote[]?> GetLotesByEventoId(int eventoId);
        Task<Lote?> GetLoteById(int eventoId, int id);
    }
}

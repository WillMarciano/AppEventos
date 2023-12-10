using appEventos.Application.Dtos;

namespace appEventos.Application.Interfaces
{
    public interface ILoteService
    {
        Task<bool> DeleteLote(int eventoId, int id);
        Task<LoteDto[]?> GetLotesByEventoId(int eventoId);
    }
}

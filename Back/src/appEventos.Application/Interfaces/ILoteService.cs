using appEventos.Application.Dtos;

namespace appEventos.Application.Interfaces
{
    public interface ILoteService
    {
        Task<bool> DeleteLoteAsync(int eventoId, int id);
        Task<LoteDto[]?> GetLotesByEventoIdAsync(int eventoId);
        Task<LoteDto?> GetLotesByIdAsync(int eventoId, int loteId);
        Task<LoteDto[]?> SaveLotesAsync(int eventoId, LoteDto[] models);
    }
}

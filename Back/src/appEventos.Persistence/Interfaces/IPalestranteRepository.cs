using appEventos.Domain.Models;

namespace appEventos.Repository.Interfaces
{
    public interface IPalestrantePersist
    {
        Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false);
        Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false);
        Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos = false);
    }
}
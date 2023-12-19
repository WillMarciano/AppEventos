using AppEventos.Domain.Models;

namespace AppEventos.Repository.Interfaces
{
    public interface IPalestranteRepository
    {
        Task<Palestrante[]?> GetAllPalestrantesAsync(bool includeEventos = false);
        Task<Palestrante[]?> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false);
        Task<Palestrante?> GetPalestranteByIdAsync(int palestranteId, bool includeEventos = false);
    }
}
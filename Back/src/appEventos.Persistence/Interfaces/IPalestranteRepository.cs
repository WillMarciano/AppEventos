using AppEventos.Domain.Models;
using AppEventos.Repository.Models;

namespace AppEventos.Repository.Interfaces
{
    public interface IPalestranteRepository : IGeralRepository
    {
        Task<PageList<Palestrante>?> GetAllPalestrantesAsync(PageParams pageParam, bool includeEventos = false);
        Task<Palestrante?> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);
    }
}
using AppEventos.Domain.Models;

namespace AppEventos.Repository.Interfaces
{
    public interface IRedeSocialRepository : IGeralRepository
    {
        Task<RedeSocial?> GetRedeSocialEventoByIdAsync(int eventoId,int id);
        Task<RedeSocial?> GetRedeSocialPalestranteByIdAsync(int palestranteId, int id);
        Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId);
    }
}

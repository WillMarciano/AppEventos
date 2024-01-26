using AppEventos.Application.Dtos;
using AppEventos.Domain.Models;
using AppEventos.Repository.Models;

namespace AppEventos.Application.Interfaces
{
    public interface IRedeSocialService
    {
        Task<RedeSocialDto[]?> GetAllEventosAsync(int id, bool isEvento);
        Task<RedeSocialDto?> GetRedeSocialByIdAsync(int id, int redeSocialId, bool isEvento);
        Task<bool> DeleteAsync(int id, int redeSocialId, bool isEvento);
        Task<RedeSocialDto[]?> SaveAsync(int id, RedeSocialDto[] models, bool isEvento);
    }
}

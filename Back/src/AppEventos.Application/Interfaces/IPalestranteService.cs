using AppEventos.Application.Dtos;
using AppEventos.Domain.Models;
using AppEventos.Repository.Models;

namespace AppEventos.Application.Interfaces
{
    public interface IPalestranteService
    {
        Task<PalestranteDto?> AddPalestranteAsync(int userId, PalestranteAddDto model);
        Task<PalestranteDto?> UpdatePalestranteAsync(int userId, PalestranteUpdateDto model);
        Task<PageList<PalestranteDto>?> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos= false);
        Task<PalestranteDto?> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);
    }
}

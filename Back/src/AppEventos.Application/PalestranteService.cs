using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using AppEventos.Domain.Models;
using AppEventos.Repository.Interfaces;
using AppEventos.Repository.Models;
using AutoMapper;

namespace AppEventos.Application
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestranteRepository _palestranteRepository;
        private readonly IMapper _mapper;

        public PalestranteService(IPalestranteRepository palestranteRepository, IMapper mapper)
        {
            _palestranteRepository = palestranteRepository;
            _mapper = mapper;
        }

        public async Task<PalestranteDto?> AddPalestranteAsync(int userId, PalestranteAddDto model)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;

                _palestranteRepository.Add<Palestrante>(palestrante);
                if (await _palestranteRepository.SaveChangesAsync())
                    return _mapper.Map<PalestranteDto>(await _palestranteRepository.GetPalestranteByUserIdAsync(palestrante.Id, false));

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto?> UpdatePalestranteAsync(int userId, PalestranteUpdateDto model)
        {
            try
            {
                var palestrante = await _palestranteRepository.GetPalestranteByUserIdAsync(userId);
                if (palestrante == null) return null;

                model.UserId = userId;

                _mapper.Map(model, palestrante);

                _palestranteRepository.Update<Palestrante>(palestrante);
                if (await _palestranteRepository.SaveChangesAsync())
                    return _mapper.Map<PalestranteDto>(await _palestranteRepository.GetPalestranteByUserIdAsync(palestrante.Id, false));

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto?> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
        {
            try
            {
                var palestrante = await _palestranteRepository.GetPalestranteByUserIdAsync(userId, includeEventos);
                if (palestrante == null) return null;

                return _mapper.Map<PalestranteDto>(palestrante);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PalestranteDto>?> GetAllPalestrantesAsync(PageParams parms, bool includeEventos = false)
        {
            try
            {
                var palestrantes = await _palestranteRepository.GetAllPalestrantesAsync(parms, includeEventos);
                if (palestrantes == null) return null;

                var resultado = _mapper.Map<PageList<PalestranteDto>>(palestrantes);

                resultado.CurrentPage = palestrantes.CurrentPage;
                resultado.TotalPages = palestrantes.TotalPages;
                resultado.PageSize = palestrantes.PageSize;
                resultado.TotalCount = palestrantes.TotalCount;

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

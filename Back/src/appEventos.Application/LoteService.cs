using appEventos.Application.Dtos;
using appEventos.Application.Interfaces;
using appEventos.Domain.Models;
using appEventos.Repository.Interfaces;
using AutoMapper;

namespace appEventos.Application
{
    public class LoteService : ILoteService
    {
        private readonly IGeralRepository _geralRepository;
        private readonly ILoteRepository _loteRepository;
        private readonly IMapper _mapper;

        public LoteService(IGeralRepository geralRepository, ILoteRepository loteRepository, IMapper mapper)
        {
            _geralRepository = geralRepository;
            _loteRepository = loteRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteLote(int eventoId, int id)
        {
            try
            {
                var lotes = await _loteRepository.GetLotesByEventoId(eventoId);
                lotes.Where(l => l.Id == id);
                if (lotes != null)
                {
                    _geralRepository.Delete<Lote[]>(lotes);
                    return await _geralRepository.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Não foi possível encontrar evento para remoção.");


                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]?> GetLotesByEventoId(int eventoId)
        {
            try
            {
                var lote = await _loteRepository.GetLotesByEventoId(eventoId);
                return lote == null ? null : _mapper.Map<LoteDto[]>(lote);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

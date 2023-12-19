using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using AppEventos.Domain.Models;
using AppEventos.Repository.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AppEventos.Application
{
    public class LoteService : ILoteService
    {
        private readonly IEventoService _eventoService;
        private readonly IGeralRepository _geralRepository;
        private readonly ILoteRepository _loteRepository;
        private readonly IMapper _mapper;

        public LoteService(IGeralRepository geralRepository,
                           ILoteRepository loteRepository,
                           IMapper mapper,
                           IEventoService eventoService)
        {
            _geralRepository = geralRepository;
            _loteRepository = loteRepository;
            _mapper = mapper;
            _eventoService = eventoService;
        }

        public async Task<bool> DeleteLoteAsync(int eventoId, int id)
        {
            try
            {
                var lote = await _loteRepository.GetLoteByIdAsync(eventoId, id);

                if (lote == null) throw new Exception("Não foi possível encontrar lote para remoção.");

                _geralRepository.Delete(lote);
                return await _geralRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto?> GetLotesByIdAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteRepository.GetLoteByIdAsync(eventoId, loteId);
                return lote == null ? null : _mapper.Map<LoteDto>(lote);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]?> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lote = await _loteRepository.GetLotesByEventoIdAsync(eventoId);
                return lote == null ? null : _mapper.Map<LoteDto[]>(lote);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]?> SaveLotesAsync(int eventoId, LoteDto[] models)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(eventoId);               

                if (evento == null) 
                    throw new Exception($"Não foi encontrado lote id:{eventoId} para atualização do Evento.");

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        model.EventoId = eventoId;
                        _geralRepository.Add<Lote>(_mapper.Map(model, new Lote()));
                    }
                    else
                    {
                        model.EventoId = eventoId;
                        _geralRepository.Update<Lote>(_mapper.Map(model, new Lote()));
                    }
                }

                if (await _geralRepository.SaveChangesAsync())
                    return _mapper.Map<LoteDto[]>(await _loteRepository.GetLotesByEventoIdAsync(eventoId));

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

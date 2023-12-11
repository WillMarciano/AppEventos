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
        private readonly IGeralRepository _geralRepository;
        private readonly ILoteRepository _loteRepository;
        private readonly IMapper _mapper;

        public LoteService(IGeralRepository geralRepository,
                           ILoteRepository loteRepository,
                           IMapper mapper)
        {
            _geralRepository = geralRepository;
            _loteRepository = loteRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteLoteAsync(int eventoId, int id)
        {
            try
            {
                var lote = await _loteRepository.GetLoteById(eventoId, id);

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
                var lote = await _loteRepository.GetLoteById(eventoId, loteId);
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
                var lote = await _loteRepository.GetLotesByEventoId(eventoId);
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
                var lotes = await _loteRepository.GetLotesByEventoId(eventoId);
                if (lotes == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        model.EventoId = eventoId;
                        _geralRepository.Add<Lote>(_mapper.Map(model, new Lote()));
                    }
                    else
                    {
                        var lote = lotes.FirstOrDefault(l => l.Id == model.Id);
                        if (lote == null) throw new Exception($"Não foi encontrado lote id:{model.Id} para atualização do Evento.");

                        model.EventoId = eventoId;
                        _geralRepository.Update<Lote>(_mapper.Map(model, lote));

                    }
                }

                if (await _geralRepository.SaveChangesAsync())
                    return _mapper.Map<LoteDto[]>(await _loteRepository.GetLotesByEventoId(eventoId));

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using appEventos.Application.Dtos;
using appEventos.Application.Interfaces;
using appEventos.Domain.Models;
using appEventos.Repository.Interfaces;
using AutoMapper;

namespace appEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralRepository _geralRepository;
        private readonly IEventoRepository _eventoRepository;
        private readonly IMapper _mapper;

        public EventoService(IGeralRepository geralRepository, IEventoRepository eventoRepository, IMapper mapper)
        {
            _geralRepository = geralRepository;
            _eventoRepository = eventoRepository;
            _mapper = mapper;
        }

        public async Task<EventoDto> AddEventos(EventoDto model)
        {
            try
            {
                var newModel = _mapper.Map<Evento>(model);
                _geralRepository.Add<Evento>(newModel);
                if (await _geralRepository.SaveChangesAsync())
                {
                    var evento =  await _eventoRepository.GetEventoByIdAsync(model.Id, false);
                    return _mapper.Map<EventoDto>(evento);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventoRepository.GetEventoByIdAsync(eventoId);
                if (evento == null) throw new Exception("Não foi possível encontrar evento para remoção.");

                _geralRepository.Delete<Evento>(evento);
                return await _geralRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto?> UpdateEvento(int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoRepository.GetEventoByIdAsync(eventoId);
                if (evento == null) return null;

                model.Id = evento.Id;

                _geralRepository.Update<EventoDto>(model);
                if (await _geralRepository.SaveChangesAsync())
                {
                    var nEvento = await _eventoRepository.GetEventoByIdAsync(model.Id, false);
                    return _mapper.Map<EventoDto>(nEvento);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]?> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoRepository.GetAllEventosAsync(includePalestrantes);
                return eventos == null ? null : _mapper.Map<EventoDto[]?>(eventos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]?> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoRepository.GetAllEventosByTemaAsync(tema, includePalestrantes);
                return eventos == null ? null : _mapper.Map<EventoDto[]?>(eventos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto?> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoRepository.GetEventoByIdAsync(eventoId, includePalestrantes);
                return evento == null ? null : _mapper.Map<EventoDto?>(evento);                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

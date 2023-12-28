using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using AppEventos.Domain.Models;
using AppEventos.Repository.Interfaces;
using AutoMapper;

namespace AppEventos.Application
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

        public async Task<EventoDto?> AddEventos(int userId, EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;

                _geralRepository.Add<Evento>(evento);
                if (await _geralRepository.SaveChangesAsync())
                    return _mapper.Map<EventoDto>(await _eventoRepository.GetEventoByIdAsync(userId, evento.Id, false));

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int userId, int eventoId)
        {
            try
            {
                var evento = await _eventoRepository.GetEventoByIdAsync(userId, eventoId);
                if (evento == null) throw new Exception("Não foi possível encontrar evento para remoção.");

                _geralRepository.Delete<Evento>(evento);
                return await _geralRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto?> UpdateEvento(int userId, int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoRepository.GetEventoByIdAsync(userId, eventoId);
                if (evento == null) return null;

                model.Id = evento.Id;
                model.UserId = userId;

                _geralRepository.Update<Evento>(_mapper.Map(model, evento));

                if (await _geralRepository.SaveChangesAsync())
                    return _mapper.Map<EventoDto>(await _eventoRepository.GetEventoByIdAsync(userId, evento.Id, false));

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]?> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoRepository.GetAllEventosAsync(userId, includePalestrantes);
                return eventos == null ? null : _mapper.Map<EventoDto[]?>(eventos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]?> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoRepository.GetAllEventosByTemaAsync(userId, tema, includePalestrantes);
                return eventos == null ? null : _mapper.Map<EventoDto[]?>(eventos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto?> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoRepository.GetEventoByIdAsync(userId, eventoId, includePalestrantes);
                return evento == null ? null : _mapper.Map<EventoDto?>(evento);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

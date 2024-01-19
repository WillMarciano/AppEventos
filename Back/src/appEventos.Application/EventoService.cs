using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using AppEventos.Domain.Models;
using AppEventos.Repository.Interfaces;
using AppEventos.Repository.Models;
using AutoMapper;

namespace AppEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly IMapper _mapper;

        public EventoService(IEventoRepository eventoRepository, IMapper mapper)
        {
            _eventoRepository = eventoRepository;
            _mapper = mapper;
        }

        public async Task<EventoDto?> AddEventos(int userId, EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;

                _eventoRepository.Add<Evento>(evento);
                if (await _eventoRepository.SaveChangesAsync())
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

                _eventoRepository.Delete<Evento>(evento);
                return await _eventoRepository.SaveChangesAsync();
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

                _eventoRepository.Update<Evento>(_mapper.Map(model, evento));

                if (await _eventoRepository.SaveChangesAsync())
                    return _mapper.Map<EventoDto>(await _eventoRepository.GetEventoByIdAsync(userId, evento.Id, false));

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<EventoDto>?> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoRepository.GetAllEventosAsync(userId, pageParams, includePalestrantes);
                if (eventos == null) return null;

                var resultado = new PageList<EventoDto>()
                {
                    CurrentPage = eventos.CurrentPage,
                    TotalPages = eventos.TotalPages,
                    PageSize = eventos.PageSize,
                    TotalCount = eventos.TotalCount
                };
                resultado.AddRange(_mapper.Map<PageList<EventoDto>>(eventos));
                return resultado;
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

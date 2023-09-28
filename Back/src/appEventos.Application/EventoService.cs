using appEventos.Application.Interfaces;
using appEventos.Domain.Models;
using appEventos.Repository.Interfaces;

namespace appEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralRepository _geralRepository;
        private readonly IEventoRepository _eventoRepository;

        public EventoService(IGeralRepository geralRepository, IEventoRepository eventoRepository)
        {
            _geralRepository = geralRepository;
            _eventoRepository = eventoRepository;
        }

        public async Task<Evento> AddEventos(Evento model)
        {
            try
            {
                _geralRepository.Add<Evento>(model);
                if (await _geralRepository.SaveChangesAsync()) 
                    return await _eventoRepository.GetEventoByIdAsync(model.Id, false);
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

        public async Task<Evento?> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                var evento = await _eventoRepository.GetEventoByIdAsync(eventoId);
                if (evento == null) return null;

                model.Id = evento.Id;

                _geralRepository.Update<Evento>(model);
                if (await _geralRepository.SaveChangesAsync())
                    return await _eventoRepository.GetEventoByIdAsync(model.Id, false);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoRepository.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoRepository.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoRepository.GetEventoByIdAsync(eventoId, includePalestrantes);
                if (evento == null) return null;

                return evento;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using appEventos.Domain.Models;
using appEventos.Repository.Context;
using appEventos.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace appEventos.Repository
{
    public class EventoRepository : IEventoRepository
    {
        public readonly AppEventosContext _context;
        public EventoRepository(AppEventosContext context) => _context = context;
        private IQueryable<Evento> FilterQueryEvento(bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e => e.Lotes).Include(e => e.RedesSociais);

            if (includePalestrantes)
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);

            query = query.OrderBy(e => e.Id);
            return query;
        }
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            var query = FilterQueryEvento(includePalestrantes);
            return await query.ToArrayAsync();
        }
        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            var query = FilterQueryEvento(includePalestrantes).Where(e => e.Tema.ToLower().Contains(tema.ToLower()));
            return await query.ToArrayAsync();
        }
        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
            => await FilterQueryEvento(includePalestrantes).Where(e => e.Id == eventoId).FirstOrDefaultAsync();
    }
}
using AppEventos.Domain.Models;
using AppEventos.Repository.Context;
using AppEventos.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppEventos.Repository
{
    public class EventoRepository : IEventoRepository
    {
        public readonly AppEventosContext _context;
        public EventoRepository(AppEventosContext context) => _context = context;
        private IQueryable<Evento> FilterQueryEvento(int userId, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e => e.Lotes).Include(e => e.RedesSociais);

            if (includePalestrantes)
                query = query.Include(e => e.PalestrantesEventos!)
                             .ThenInclude(pe => pe.Palestrante);

            return query.AsNoTracking()
                         .Where(x => x.UserId == userId)
                         .OrderBy(e => e.Id);
        }

        public async Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes = false) 
            => await FilterQueryEvento(userId, includePalestrantes).ToArrayAsync();

        public async Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false) 
            => await FilterQueryEvento(userId, includePalestrantes).Where(e => e.Tema!.ToLower().Contains(tema.ToLower())).ToArrayAsync();

        public async Task<Evento?> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false) 
            => await FilterQueryEvento(userId, includePalestrantes).Where(e => e.Id == eventoId).FirstOrDefaultAsync() ?? null;
    }
}
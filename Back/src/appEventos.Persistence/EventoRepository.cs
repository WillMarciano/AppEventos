using AppEventos.Domain.Models;
using AppEventos.Repository.Context;
using AppEventos.Repository.Interfaces;
using AppEventos.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace AppEventos.Repository
{
    public class EventoRepository : IEventoRepository
    {
        public readonly AppEventosContext _context;
        public EventoRepository(AppEventosContext context) => _context = context;
        private IQueryable<Evento> FilterQueryEvento(int userId, bool includePalestrantes, PageParams pageParams)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e => e.Lotes).Include(e => e.RedesSociais);

            if (includePalestrantes)
                query = query.Include(e => e.PalestrantesEventos!)
                             .ThenInclude(pe => pe.Palestrante);

            if (!string.IsNullOrEmpty(pageParams.Term))
                return query.AsNoTracking()
                .Where(e => (e.Tema!.ToLower().Contains(pageParams.Term.ToLower())
                            || e.Local!.ToLower().Contains(pageParams.Term.ToLower()))
                && e.UserId == userId)
                .OrderBy(e => e.Id);

            return query.AsNoTracking()
                         .Where(x => x.UserId == userId)
                         .OrderBy(e => e.Id);
        }

        public async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false)
        {
            var query = FilterQueryEvento(userId, includePalestrantes, pageParams);
            return await PageList<Evento>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }
        public async Task<Evento?> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
            => await FilterQueryEvento(userId, includePalestrantes, new PageParams()).Where(e => e.Id == eventoId).FirstOrDefaultAsync() ?? null;

    }
}
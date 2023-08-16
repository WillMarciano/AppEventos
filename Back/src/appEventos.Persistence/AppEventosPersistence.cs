using appEventos.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace appEventos.Persistence
{
    public class AppEventosPersistence : IAppEventosPersistence
    {
        public readonly AppEventosContext _context;
        public AppEventosPersistence(AppEventosContext context) => _context = context;

        #region Geral
        public void Add<T>(T entity) where T : class => _context.Add(entity);

        public void Delete<T>(T entity) where T : class => _context.Remove(entity);

        public void DeleteRange<T>(T[] entity) where T : class => _context.RemoveRange(entity);

        public void Update<T>(T entity) where T : class => _context.Update(entity);
        public async Task<bool> SaveChangesAsync()
        {
            //Se o retorno dele for mair que 0, foi realizada alguma alteração
            return (await _context.SaveChangesAsync()) > 0;
        }

        #endregion

        #region Evento
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
        #endregion

        #region Palestrante
        private IQueryable<Palestrante> FilterQueryPalestrante(bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(p => p.RedesSociais);

            if (includeEventos)
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(p => p.Evento);

            query = query.OrderBy(e => e.Id);
            return query;
        }
        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            var query = FilterQueryPalestrante(includeEventos);
            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
        {
            var query = FilterQueryPalestrante(includeEventos).Where(e => e.Nome.ToLower().Contains(nome.ToLower()));
            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos = false)
            => await FilterQueryPalestrante(includeEventos).Where(e => e.Id == palestranteId).FirstOrDefaultAsync();
        #endregion
    }
}
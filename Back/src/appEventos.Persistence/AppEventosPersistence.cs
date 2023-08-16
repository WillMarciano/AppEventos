using appEventos.Domain.Models;

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
        public Task<Evento[]> GetAllEventosAsync(bool includePalestrantes)
        {
            throw new NotImplementedException();
        }

        public Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes)
        {
            throw new NotImplementedException();
        }
        public Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Palestrante
        public Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos)
        {
            throw new NotImplementedException();
        }

        public Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos)
        {
            throw new NotImplementedException();
        }

        public Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
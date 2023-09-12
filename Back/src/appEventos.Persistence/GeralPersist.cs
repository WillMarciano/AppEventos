using appEventos.Persistence.Interfaces;

namespace appEventos.Persistence
{
    public class GeralPersist : IGeralPersist
    {
        public readonly AppEventosContext _context;
        public GeralPersist(AppEventosContext context) => _context = context;

        public void Add<T>(T entity) where T : class => _context.Add(entity);

        public void Delete<T>(T entity) where T : class => _context.Remove(entity);

        public void DeleteRange<T>(T[] entity) where T : class => _context.RemoveRange(entity);

        public void Update<T>(T entity) where T : class => _context.Update(entity);
        public async Task<bool> SaveChangesAsync()
        {
            //Se o retorno dele for mair que 0, foi realizada alguma alteração
            return (await _context.SaveChangesAsync()) > 0;
        }

    }
}
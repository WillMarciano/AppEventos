using appEventos.Repository.Interfaces;

namespace appEventos.Repository
{
    public class GeralRepository : IGeralRepository
    {
        public readonly AppEventosContext _context;
        public GeralRepository(AppEventosContext context) => _context = context;

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
using appEventos.Domain.Models;
using appEventos.Repository.Context;
using appEventos.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace appEventos.Repository
{
    public class PalestranteRepository : IPalestranteRepository
    {
        public readonly AppEventosContext _context;
        public PalestranteRepository(AppEventosContext context) => _context = context;

        private IQueryable<Palestrante> FilterQueryPalestrante(bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(p => p.RedesSociais);

            if (includeEventos)
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(p => p.Evento);

            query = query.OrderBy(e => e.Id);
            return query.AsNoTracking();
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

    }
}
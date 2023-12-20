using AppEventos.Domain.Models;
using AppEventos.Repository.Context;
using AppEventos.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppEventos.Repository
{
    public class PalestranteRepository : IPalestranteRepository
    {
        public readonly AppEventosContext _context;
        public PalestranteRepository(AppEventosContext context) => _context = context;

        private IQueryable<Palestrante> FilterQueryPalestrante(bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(p => p.RedesSociais);

            if (includeEventos)
                query = query.Include(p => p.PalestrantesEventos)
                             .ThenInclude(p => p.Evento);

            query = query.OrderBy(e => e.Id);
            return query.AsNoTracking();
        }
        public async Task<Palestrante[]?> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            var query = FilterQueryPalestrante(includeEventos);
            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]?> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
        {
            return await FilterQueryPalestrante(includeEventos)
                .Include(e => e.User)
                .Where(e => e.User!.Nome.ToLower().Contains(nome.ToLower()) || 
                            e.User!.Sobrenome.ToLower().Contains(nome.ToLower())
                ).ToArrayAsync();
        }

        public async Task<Palestrante?> GetPalestranteByIdAsync(int palestranteId, bool includeEventos = false)
            => await FilterQueryPalestrante(includeEventos).Where(e => e.Id == palestranteId).FirstOrDefaultAsync();

    }
}
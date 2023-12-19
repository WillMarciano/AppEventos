using AppEventos.Domain.Models;
using AppEventos.Repository.Context;
using AppEventos.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppEventos.Repository
{
    public class LoteRepository : ILoteRepository
    {
        public readonly AppEventosContext _context;

        public LoteRepository(AppEventosContext context) => _context = context;
        private IQueryable<Lote> FilterQuery(int eventoId)
        {
            IQueryable<Lote> teste = _context.Lotes;
            IQueryable<Lote> query = _context.Lotes.Where(l => l.EventoId == eventoId);

            query = query.OrderBy(e => e.Id);
            return query.AsNoTracking();
        }

        public async Task<Lote[]?> GetLotesByEventoIdAsync(int eventoId)
        {
            var lotes = await FilterQuery(eventoId).ToArrayAsync();
            if (lotes.Count() > 0)
                return lotes;
            return null;
        }

        public async Task<Lote?> GetLoteByIdAsync(int eventoId, int id)
        {
            var lote = await FilterQuery(eventoId).Where(l => l.Id == id).FirstOrDefaultAsync() ?? null;
            return lote;
        }
    }
}

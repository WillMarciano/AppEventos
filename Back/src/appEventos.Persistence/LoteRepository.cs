using appEventos.Domain.Models;
using appEventos.Repository.Context;
using appEventos.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace appEventos.Repository
{
    public class LoteRepository : ILoteRepository
    {
        public readonly AppEventosContext _context;

        public LoteRepository(AppEventosContext context) => _context = context;
        private IQueryable<Lote> FilterQuery(int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes.Where(l => l.EventoId == eventoId);

            query = query.OrderBy(e => e.Id);
            return query.AsNoTracking();
        }

        public async Task<Lote[]> GetLotesByEventoId(int eventoId)
        {
            var lote = await FilterQuery(eventoId).ToArrayAsync();
            return lote;
        }
    }
}

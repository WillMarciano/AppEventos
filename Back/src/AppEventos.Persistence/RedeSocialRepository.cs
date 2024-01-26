using AppEventos.Domain.Models;
using AppEventos.Repository.Context;
using AppEventos.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppEventos.Repository
{
    public class RedeSocialRepository : GeralRepository, IRedeSocialRepository
    {
        public new readonly AppEventosContext _context;

        public RedeSocialRepository(AppEventosContext context) : base(context) => _context = context;

        private IQueryable<RedeSocial> FilterQueryPalestrante()
        {
            IQueryable<RedeSocial> query = _context.RedesSocials
                                            .Include(p => p.Palestrante)
                                            .Include(e => e.Evento)
                                            .OrderBy(p => p.Id)
                                            .AsNoTracking();

            return query;
        }

        public async Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId) 
            => await FilterQueryPalestrante()
            .Where(rs => rs.EventoId == eventoId)
            .ToArrayAsync();

        public async Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId) 
            => await FilterQueryPalestrante()
            .Where(rs => rs.PalestranteId == palestranteId)
            .ToArrayAsync();

        public async Task<RedeSocial?> GetRedeSocialEventoByIdAsync(int eventoId, int id)
            => await FilterQueryPalestrante()
            .Where(rs => rs.EventoId == eventoId && rs.Id == id)
            .FirstOrDefaultAsync();

        public async Task<RedeSocial?> GetRedeSocialPalestranteByIdAsync(int palestranteId, int id) 
            => await FilterQueryPalestrante()
            .Where(rs => rs.PalestranteId == palestranteId && rs.Id == id)
            .FirstOrDefaultAsync();
    }
}

using AppEventos.Domain.Identity;
using AppEventos.Domain.Models;
using AppEventos.Repository.Context;
using AppEventos.Repository.Interfaces;
using AppEventos.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace AppEventos.Repository
{
    public class PalestranteRepository : GeralRepository, IPalestranteRepository
    {
        public new readonly AppEventosContext _context;
        public PalestranteRepository(AppEventosContext context) : base(context) => _context = context;

        private IQueryable<Palestrante> FilterQueryPalestrante(PageParams pageParams, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                                            .Include(p => p.User)
                                            .Include(p => p.RedesSociais)
                                            .Where(p => p.User!.Funcao == Domain.Enum.Funcao.Palestrante);

            if (includeEventos)
                query = query.Include(p => p.PalestrantesEventos!)
                             .ThenInclude(p => p.Evento);

            if (!string.IsNullOrEmpty(pageParams.Term))
                query = query.Where(p => (p.MiniCurriculo!.ToLower().Contains(pageParams.Term!.ToLower()) ||
                                      p.User!.Nome.ToLower().Contains(pageParams.Term!.ToLower()) ||
                                      p.User.Sobrenome.ToLower().Contains(pageParams.Term!.ToLower())))
                             .OrderBy(p => p.Id);


            return query.AsNoTracking();
        }
        public async Task<PageList<Palestrante>?> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            var query = FilterQueryPalestrante(pageParams, includeEventos);
            return await PageList<Palestrante>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }
        public async Task<Palestrante?> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
            => await FilterQueryPalestrante(new PageParams(), includeEventos).Where(e => e.UserId == userId).FirstOrDefaultAsync();

    }
}
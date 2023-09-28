using appEventos.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace appEventos.Repository
{
    public class AppEventosContext : DbContext
    {
        public AppEventosContext(DbContextOptions<AppEventosContext> options) : base(options) { }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }
        public DbSet<RedeSocial> RedesSocials { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PalestranteEvento>()
                .HasKey(pe => new { pe.EventoId, pe.PalestranteId });

        }
    }
}

using appEventos.API.Models;
using Microsoft.EntityFrameworkCore;

namespace appEventos.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base() { }

        public DbSet<Evento> Eventos { get; set; }
    }
}

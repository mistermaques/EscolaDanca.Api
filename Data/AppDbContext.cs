using Microsoft.EntityFrameworkCore;
using EscolaDanca.Api.Models;



namespace EscolaDanca.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Aula> Aulas { get; set; }
        public DbSet<AulaHorario> HorariosAulas { get; set; }

        public DbSet<Plano> Planos { get; set; }
        public DbSet<PlanoAula> PlanosAulas { get; set; }

    }
}

using Microsoft.EntityFrameworkCore;
using EscolaDanca.Api.Models;
using static EscolaDanca.Api.Models.Usuario;



namespace EscolaDanca.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Aula> Aulas { get; set; }
        public DbSet<AulaHorario> HorariosAulas { get; set; }

        public DbSet<UsuarioAula> UsuariosAulas { get; set; }
        public DbSet<AlunoAula> AlunosAulas { get; set; }

        public DbSet<Pagamento> Pagamentos { get; set; }



    }
}

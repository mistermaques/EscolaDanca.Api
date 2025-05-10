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

        public DbSet<UsuarioAula> UsuariosAulas { get; set; }
        public DbSet<AlunoAula> AlunosAulas { get; set; }

        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("usuarios");
            modelBuilder.Entity<Pagamento>().ToTable("pagamentos");
            modelBuilder.Entity<Aula>().ToTable("aulas");
            modelBuilder.Entity<AulaHorario>().ToTable("aulahorarios");
            modelBuilder.Entity<UsuarioAula>().ToTable("usuarioaula");
            modelBuilder.Entity<AlunoAula>().ToTable("alunoaula");
            // 👈 em minúsculo
        }


    }

}

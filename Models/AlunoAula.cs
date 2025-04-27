namespace EscolaDanca.Api.Models
{
    public class AlunoAula
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public int AulaId { get; set; }
        public Aula Aula { get; set; } = null!;

        public DateTime DataAssinatura { get; set; } = DateTime.UtcNow;
    }
}

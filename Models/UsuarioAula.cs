using System.ComponentModel.DataAnnotations.Schema;

namespace EscolaDanca.Api.Models
{
    [Table("usuarioaula")]
    public class UsuarioAula
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("usuarioid")]
        public int UsuarioId { get; set; }
        [ForeignKey("usuarioid")]
        public Usuario Usuario { get; set; } = null!;
        [Column("aulaid")]
        public int AulaId { get; set; }
        [ForeignKey("aulaid")]
        public Aula Aula { get; set; } = null!;
        [Column("dataassinatura")]
        public DateTime DataAssinatura { get; set; }
        [Column("validadeassinatura")]
        public DateTime ValidadeAssinatura { get; set; }
    }
}

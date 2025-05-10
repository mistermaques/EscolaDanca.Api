using System.ComponentModel.DataAnnotations.Schema;

namespace EscolaDanca.Api.Models
{
    [Table("pagamentos")]
    public class Pagamento
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("alunoid")]
        public int AlunoId { get; set; }

        [ForeignKey("AlunoId")]
        public Usuario Aluno { get; set; } = null!;

        [Column("aulaid")]
        public int AulaId { get; set; }

        [ForeignKey("AulaId")]
        public Aula Aula { get; set; } = null!;

        [Column("valor")]
        public decimal Valor { get; set; }

        [Column("datapagamento")]
        public DateTime DataPagamento { get; set; }

        [Column("metodopagamento")]
        public string MetodoPagamento { get; set; } = string.Empty;
    }
}

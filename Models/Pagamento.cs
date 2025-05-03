namespace EscolaDanca.Api.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public int AulaId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public string MetodoPagamento { get; set; } = string.Empty;

        // Relacionamentos (opcional para facilitar os joins)
        public Usuario Aluno { get; set; }
        public Aula Aula { get; set; }
    }

}

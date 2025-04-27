namespace EscolaDanca.Api.Dtos
{
    public class PedidoPagamentoDto
    {
        public int UsuarioId { get; set; }
        public int AulaId { get; set; } 
        public string Titulo { get; set; } = "";
        public decimal Valor { get; set; }
    }
}

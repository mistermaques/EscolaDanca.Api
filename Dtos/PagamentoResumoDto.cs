namespace EscolaDanca.Api.Dtos
{
    public class PagamentoResumoDto
    {
        public List<PagamentoDetalhadoDto> Pagamentos { get; set; } = new();
        public decimal TotalPeriodo { get; set; }
        public string AulaMaisVendida { get; set; } = "";
        public string AulaMaisAssinada { get; set; } = "";
    }

    public class PagamentoDetalhadoDto
    {
        public string AlunoName { get; set; } = "";
        public string AulaNome { get; set; } = "";
        public string MetodoPagamento { get; set; } = "";
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
    }

}

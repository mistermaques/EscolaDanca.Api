namespace EscolaDanca.Api.Dtos
{
    public class PlanoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Descricao { get; set; } = "";
        public decimal ValorMensal { get; set; }
        public List<AulaDto> Aulas { get; set; } = new();
    }

}

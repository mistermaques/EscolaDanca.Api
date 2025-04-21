namespace EscolaDanca.Api.Dtos
{
    public class PlanoCadastroDto
    {
        public string Nome { get; set; } = "";
        public string Descricao { get; set; } = "";
        public decimal ValorMensal { get; set; }
        public List<int> AulasSelecionadas { get; set; } = new();
    }

}

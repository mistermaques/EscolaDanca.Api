namespace EscolaDanca.Api.Dtos

{
    public class AulaCadastroDto
    {
        public string Nome { get; set; } = "";
        public string Nivel { get; set; } = "";
        public string Descricao { get; set; } = "";
        public string Tipo { get; set; } = "";
        public int ProfessorId { get; set; }
        public decimal ValorMensal { get; set; }

        public Dictionary<string, List<string>> HorariosPorDia { get; set; } = new();
    }
}

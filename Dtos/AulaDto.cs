namespace EscolaDanca.Api.Dtos
{
    public class AulaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Descricao { get; set; } = "";
        public string Nivel { get; set; } = "";
        public string Tipo { get; set; } = "";
        public string DiaSemana { get; set; } = "";
        public string Horario { get; set; } = "";
        public int ProfessorId { get; set; }
        public decimal ValorMensal { get; set; }
    }
}

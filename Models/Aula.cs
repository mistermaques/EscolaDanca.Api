namespace EscolaDanca.Api.Models
{
    public class Aula
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Descricao { get; set; } = "";
        public string Nivel { get; set; } = "";
        public string Tipo { get; set; } = "";
        public int ProfessorId { get; set; }

        public List<PlanoAula> Planos { get; set; } = new();


        // Navegação
        public List<AulaHorario> Horarios { get; set; } = new();
    }

    public class AulaHorario
    {
        public int Id { get; set; }
        public string DiaSemana { get; set; } = "";
        public string Horario { get; set; } = "";

        public int AulaId { get; set; }
        public Aula Aula { get; set; } = null!;
    }

}

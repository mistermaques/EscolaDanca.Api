using System.ComponentModel.DataAnnotations.Schema;

namespace EscolaDanca.Api.Models
{
    public class Aula
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; } = "";

        [Column("descricao")]
        public string Descricao { get; set; } = "";

        [Column("nivel")]
        public string Nivel { get; set; } = "";

        [Column("tipo")]
        public string Tipo { get; set; } = "";

        [Column("professor_id")]
        public int? ProfessorId { get; set; }

        public Usuario? Professor { get; set; }

        [Column("valor_mensal")]
        public decimal ValorMensal { get; set; }

        public List<AulaHorario> Horarios { get; set; } = new();
    }

    public class AulaHorario
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("diasemana")]
        public string DiaSemana { get; set; } = "";

        [Column("horario")]
        public string Horario { get; set; } = "";

        [Column("aulaid")]
        public int AulaId { get; set; }

        public Aula Aula { get; set; } = null!;
    }
}

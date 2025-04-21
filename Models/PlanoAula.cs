namespace EscolaDanca.Api.Models
{
    public class PlanoAula
    {
        public int Id { get; set; }

        public int PlanoId { get; set; }
        public Plano Plano { get; set; } = null!;

        public int AulaId { get; set; }
        public Aula Aula { get; set; } = null!;
    }

}

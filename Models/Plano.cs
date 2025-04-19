using Microsoft.EntityFrameworkCore;

namespace EscolaDanca.Api.Models
{
    public class Plano
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        [Precision(10, 2)] // até 99999999.99
        public decimal Valor { get; set; }
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }

}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscolaDanca.Api.Models
{
    [Table("Planos")]
    public class Plano
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        [Precision(10, 2)] // até 99999999.99
        public decimal ValorMensal { get; set; }
        public bool Ativo { get; set; } = true;


        public List<PlanoAula> Aulas { get; set; } = new();
    }

}

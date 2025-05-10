using System.ComponentModel.DataAnnotations.Schema;

namespace EscolaDanca.Api.Models
{
    public class Usuario
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("senha_hash")]
        public string SenhaHash { get; set; } = string.Empty;

        [Column("tipo_usuario")]
        public string TipoUsuario { get; set; } = string.Empty;

        [Column("status")]
        public string Status { get; set; } = string.Empty;

        [Column("cpf")]
        public string Cpf { get; set; } = string.Empty;

        [Column("telefone")]
        public string Telefone { get; set; } = string.Empty;

        [Column("endereco")]
        public string Endereco { get; set; } = string.Empty;

        [Column("foto_url")]
        public string? FotoUrl { get; set; }

        [Column("primeiro_acesso")]
        public bool PrimeiroAcesso { get; set; } = true;

        public List<UsuarioAula> AulasAssinadas { get; set; } = new();
    }
}

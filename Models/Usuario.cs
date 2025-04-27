namespace EscolaDanca.Api.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public string TipoUsuario { get; set; } 
        public string Status { get; set; }

        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string? FotoUrl { get; set; }
        public bool PrimeiroAcesso { get; set; } = true;
        public List<UsuarioAula> AulasAssinadas { get; set; } = new();

        public class UsuarioAula
        {
            public int Id { get; set; }

            public int UsuarioId { get; set; }
            public Usuario Usuario { get; set; } = null!;

            public int AulaId { get; set; }
            public Aula Aula { get; set; } = null!;

            public DateTime DataAssinatura { get; set; }
            public DateTime ValidadeAssinatura { get; set; }
        }
        
    }
}

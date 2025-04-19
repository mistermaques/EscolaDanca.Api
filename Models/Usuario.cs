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

        // Relacionamento com Plano
        public int? PlanoId { get; set; }  // pode ser nulo para professor/funcionário
        public Plano? Plano { get; set; }

        public string Cpf { get; set; } 
        public string Telefone { get; set; } 
        public string Endereco { get; set; } 
        public string? FotoUrl { get; set; }
        public bool PrimeiroAcesso { get; set; } = true;


    }
}

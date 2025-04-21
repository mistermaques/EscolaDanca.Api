namespace EscolaDanca.Api.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public string TipoUsuario { get; set; } // Aluno, Professor, Administrativo
        public string Status { get; set; }

        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string? FotoUrl { get; set; }
        public bool PrimeiroAcesso { get; set; } = true;

        // 🔐 Pagamento e assinatura
        public int? PlanoId { get; set; }
        public Plano? Plano { get; set; }
        public DateTime? DataAssinatura { get; set; }
        public DateTime? ValidadeAssinatura { get; set; }
        public string StatusPagamento { get; set; } = "Aguardando"; // Pago, Aguardando, Cancelado
    }
}

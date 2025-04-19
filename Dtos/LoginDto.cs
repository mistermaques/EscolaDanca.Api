namespace EscolaDanca.Api.Dtos
{
    public class LoginDto
    {
        public string Email { get; set; } = String.Empty;
        public string Senha { get; set; } = String.Empty;
        public bool ForceChangePassword { get; set; }
        public int UserId { get; set; }

    }
}

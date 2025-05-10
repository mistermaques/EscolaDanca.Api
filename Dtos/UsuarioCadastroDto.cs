namespace EscolaDanca.Api.Dtos
{
    public class UsuarioCadastroDto
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Senha { get; set; } = ""; // será criptografada na API
        public string TipoUsuario { get; set; } = ""; // "Aluno", "Professor", etc.
        public string Status { get; set; } = "Ativo"; // padrão já ativo

        public string Cpf { get; set; } = "";
        public string Telefone { get; set; } = "";
        public string Endereco { get; set; } = "";
        public string FotoUrl { get; set; } = "";

    }
}

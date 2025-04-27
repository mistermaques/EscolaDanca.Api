namespace EscolaDanca.Api.Dtos
{
    public class UsuarioAulaDto
    {
        public int AulaId { get; set; }
        public string NomeAula { get; set; } = "";
        public DateTime DataAssinatura { get; set; }
        public DateTime ValidadeAssinatura { get; set; }
    }
}

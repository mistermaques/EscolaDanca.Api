namespace EscolaDanca.Api.Dtos
{
    public class ResetPasswordDto
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; } = "";
    }
}

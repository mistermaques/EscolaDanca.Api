using Microsoft.AspNetCore.Mvc;
using EscolaDanca.Api.Data;
using EscolaDanca.Api.Dtos;
using EscolaDanca.Api.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EscolaDanca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResetPasswordController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResetPasswordController(AppDbContext context)
        {
            _context = context;
        }

        // POST api/resetpassword/reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            // 1) Procura o usuário pelo ID
            var user = await _context.Usuarios.FindAsync(dto.UserId);
            if (user == null)
                return NotFound(new { Mensagem = "Usuário não encontrado." });

            // 2) Gera o hash da nova senha
            user.SenhaHash = GerarHash(dto.NewPassword);
            user.PrimeiroAcesso = false;

            // 3) Persiste no banco
            await _context.SaveChangesAsync();

            // 4) Retorna OK
            return Ok(new { Mensagem = "Senha redefinida com sucesso." });
        }

        // Helper para criar SHA256 hash
        private string GerarHash(string senha)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}

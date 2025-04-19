using Microsoft.AspNetCore.Mvc;
using EscolaDanca.Api.Data;
using EscolaDanca.Api.Dtos;
using EscolaDanca.Api.Models;
using System.Security.Cryptography;
using System.Text;

namespace EscolaDanca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
            => _context = context;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            // 1) Gera o hash da senha recebida
            var senhaHash = GerarHash(dto.Senha);

            // 2) Busca usuário por email + hash
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == dto.Email && u.SenhaHash == senhaHash);

            // 3) Se não existir, retorna 401
            if (usuario == null)
                return Unauthorized(new { mensagem = "Usuário ou senha inválidos" });

            // 4) Se for o PRIMERIO ACESSO, sinaliza para o front forçar troca
            if (usuario.PrimeiroAcesso)
                return Ok(new
                {
                    ForceChangePassword = true,
                    UserId = usuario.Id,
                    Mensagem = "Primeiro acesso — defina sua nova senha."
                });

            // 5) Fluxo normal de login
            return Ok(new
            {
                mensagem = "Login realizado com sucesso",
                usuario.Id,
                usuario.Name,
                usuario.Email,
                usuario.TipoUsuario
                // aqui você pode também gerar e retornar um JWT…
            });
        }

        [HttpPost("validar-senha")]
        public IActionResult ValidarSenha([FromBody] ValidarSenhaDto dto)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == dto.Email);

            if (usuario == null || usuario.TipoUsuario != "Administrador")
                return Unauthorized(new { mensagem = "Usuário não autorizado." });

            // Gerar hash da senha digitada
            using var sha = SHA256.Create();
            var senhaHash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Senha)));

            if (usuario.SenhaHash != senhaHash)
                return Unauthorized(new { mensagem = "Senha incorreta." });

            return Ok(new { mensagem = "Senha confirmada." });
        }


        // Mesma função que você já tinha em UsuariosController
        private string GerarHash(string senha)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}

using EscolaDanca.Api.Data;
using EscolaDanca.Api.Dtos;
using EscolaDanca.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EscolaDanca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;


        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] UsuarioCadastroDto dto)
        {
            // Verifica se o e-mail já está em uso
            var existe = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
            
            if (existe)
            {
                return BadRequest(new { Mensagem = "E-mail já cadastrado." });
            }

            // Criptografa a senha
            var senhaTemp = Guid.NewGuid().ToString("N").Substring(0, 8);
            string senhaHash = GerarHash(senhaTemp);


            // Cria o objeto de usuário
            var usuario = new Usuario
            {
                Name = dto.Name,
                Email = dto.Email,
                SenhaHash = senhaHash,
                PrimeiroAcesso = true,
                TipoUsuario = dto.TipoUsuario,
                Status = dto.Status,
                //PlanoId = dto.PlanoId, // pode ser nulo
                Cpf = dto.Cpf,
                Telefone = dto.Telefone,
                Endereco = dto.Endereco,
                FotoUrl = dto.FotoUrl,
            };

            // Salva no banco
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { Mensagem = "Usuário cadastrado com sucesso.",
            SenhaTemporaria = senhaTemp
            });



        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Select(u => new {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.TipoUsuario,
                    u.Status
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            var dto = new UsuarioCadastroDto
            {
                Name = usuario.Name,
                Email = usuario.Email,
                TipoUsuario = usuario.TipoUsuario,
                Status = usuario.Status,
                //PlanoId = usuario.PlanoId,
                Cpf = usuario.Cpf,
                Telefone = usuario.Telefone,
                Endereco = usuario.Endereco,
                FotoUrl = usuario.FotoUrl
                // Senha continua sendo manipulada apenas internamente
            };

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuario(int id, [FromBody] UsuarioCadastroDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            // Atualiza os dados (exceto senha)
            usuario.Name = dto.Name;
            usuario.Email = dto.Email;
            usuario.TipoUsuario = dto.TipoUsuario;
            usuario.Status = dto.Status;
            //usuario.PlanoId = dto.PlanoId;
            usuario.Cpf = dto.Cpf;
            usuario.Telefone = dto.Telefone;
            usuario.Endereco = dto.Endereco;
            usuario.FotoUrl = dto.FotoUrl;

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Usuário atualizado com sucesso!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Usuário excluído com sucesso." });
        }

        [HttpGet("alunos")]
        public async Task<ActionResult<List<UsuarioDto>>> ListarAlunos()
        {
            var alunos = await _context.Usuarios
                .Where(u => u.TipoUsuario == "Aluno")
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Nome = u.Name
                })
                .ToListAsync();

            return Ok(alunos);
        }




        // Função auxiliar para gerar hash SHA256
        private string GerarHash(string senha)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}

using EscolaDanca.Api.Dtos;
using EscolaDanca.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EscolaDanca.Api.Data; // Certifique-se de importar seu DbContext

namespace EscolaDanca.Api.Controllers
{
    [ApiController]
    [Route("api/planos")]
    public class PlanoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlanoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarPlano([FromBody] PlanoCadastroDto dto)
        {
            // 🔒 Validação para evitar IDs inválidos (ex: 0 ou negativos)
            if (dto.AulasSelecionadas.Any(id => id <= 0))
            {
                return BadRequest("IDs de aulas inválidos detectados.");
            }

            // 💡 (Opcional) validar se todas as aulas realmente existem no banco
            var aulasValidas = await _context.Aulas
                .Where(a => dto.AulasSelecionadas.Contains(a.Id))
                .Select(a => a.Id)
                .ToListAsync();

            if (aulasValidas.Count != dto.AulasSelecionadas.Count)
            {
                return BadRequest("Uma ou mais aulas selecionadas não existem.");
            }

            // Criação do plano
            var plano = new Plano
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                ValorMensal = dto.ValorMensal,
                Ativo = true,
                Aulas = aulasValidas.Select(id => new PlanoAula
                {
                    AulaId = id
                }).ToList()
            };

            _context.Planos.Add(plano);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpGet("listar")]
        public async Task<ActionResult<List<PlanoDto>>> Listar()
        {
            var planos = await _context.Planos
                .Include(p => p.Aulas)
                .ThenInclude(pa => pa.Aula)
                .ToListAsync();

            var resultado = planos.Select(p => new PlanoDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                ValorMensal = p.ValorMensal,
                Aulas = p.Aulas.Select(pa => new AulaDto
                {
                    Id = pa.Aula.Id,
                    Nome = pa.Aula.Nome,
                    Nivel = pa.Aula.Nivel,
                    Tipo = pa.Aula.Tipo
                }).ToList()
            }).ToList();

            return Ok(resultado);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirPlano(int id)
        {
            var plano = await _context.Planos
                .Include(p => p.Aulas) // Inclui a relação para garantir exclusão
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plano == null)
                return NotFound("Plano não encontrado.");

            _context.Planos.Remove(plano);
            await _context.SaveChangesAsync();

            return NoContent(); // Ou return Ok("Plano excluído com sucesso.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlanoDto>> BuscarPorId(int id)
        {
            var plano = await _context.Planos
                .Include(p => p.Aulas)
                .ThenInclude(pa => pa.Aula)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plano == null)
                return NotFound("Plano não encontrado.");

            var resultado = new PlanoDto
            {
                Id = plano.Id,
                Nome = plano.Nome,
                Descricao = plano.Descricao,
                ValorMensal = plano.ValorMensal,
                Aulas = plano.Aulas.Select(pa => new AulaDto
                {
                    Id = pa.Aula.Id,
                    Nome = pa.Aula.Nome,
                    Nivel = pa.Aula.Nivel,
                    Tipo = pa.Aula.Tipo
                }).ToList()
            };

            return Ok(resultado);
        }







    }
}

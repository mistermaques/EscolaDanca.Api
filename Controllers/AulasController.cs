using EscolaDanca.Api.Data;
using EscolaDanca.Api.Dtos;
using EscolaDanca.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EscolaDanca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AulasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AulasController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/aulas/cadastrar
        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] AulaCadastroDto dto)
        {
            var aula = new Aula
            {
                Nome = dto.Nome,
                Nivel = dto.Nivel,
                Descricao = dto.Descricao,
                Tipo = dto.Tipo,
                ProfessorId = dto.ProfessorId,
                Horarios = dto.HorariosPorDia.SelectMany(kvp =>
                    kvp.Value.Select(hora => new AulaHorario
                    {
                        DiaSemana = kvp.Key,
                        Horario = hora
                    })).ToList()
            };

            _context.Aulas.Add(aula);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Aula cadastrada com sucesso!" });
        }


        // GET: api/aulas/listar
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<Aula>>> Listar()
        {
            return await _context.Aulas.ToListAsync();
        }

        [HttpGet("horarios-ocupados")]
        public IActionResult GetHorariosOcupados()
        {
            var resultado = _context.Aulas
                .SelectMany(a => a.Horarios)
                .Select(h => new
                {
                    h.DiaSemana,
                    h.Horario
                })
                .ToList();

            var horariosPorDia = new Dictionary<string, List<string>>();

            foreach (var item in resultado)
            {
                if (!horariosPorDia.ContainsKey(item.DiaSemana))
                    horariosPorDia[item.DiaSemana] = new List<string>();

                horariosPorDia[item.DiaSemana].Add(item.Horario);
            }

            return Ok(horariosPorDia);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var aula = await _context.Aulas
                .Include(a => a.Horarios) // inclui os horários vinculados
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aula == null)
                return NotFound(new { mensagem = "Aula não encontrada." });

            // Remove os horários antes (devido à relação 1:N)
            _context.HorariosAulas.RemoveRange(aula.Horarios);

            // Remove a aula em si
            _context.Aulas.Remove(aula);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Aula excluída com sucesso!" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var aula = await _context.Aulas
                .Include(a => a.Horarios)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aula == null)
                return NotFound();

            var dto = new
            {
                aula.Nome,
                aula.Nivel,
                aula.Tipo,
                aula.Descricao,
                aula.ProfessorId,
                HorariosPorDia = aula.Horarios
                    .GroupBy(h => h.DiaSemana)
                    .ToDictionary(g => g.Key, g => g.Select(h => h.Horario).ToList())
            };

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarAula(int id, [FromBody] AulaCadastroDto dto)
        {
            var aulaExistente = await _context.Aulas
                .Include(a => a.Horarios)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aulaExistente is null)
                return NotFound("Aula não encontrada.");

            // Atualiza os dados básicos
            aulaExistente.Nome = dto.Nome;
            aulaExistente.Nivel = dto.Nivel;
            aulaExistente.Tipo = dto.Tipo;
            aulaExistente.Descricao = dto.Descricao;
            aulaExistente.ProfessorId = dto.ProfessorId;

            // Remove os horários antigos
            _context.HorariosAulas.RemoveRange(aulaExistente.Horarios);

            // Adiciona os novos horários
            aulaExistente.Horarios = dto.HorariosPorDia
                .SelectMany(kvp => kvp.Value.Select(hora => new AulaHorario
                {
                    DiaSemana = kvp.Key,
                    Horario = hora,
                    AulaId = id
                }))
                .ToList();

            await _context.SaveChangesAsync();

            return Ok("Aula atualizada com sucesso!");
        }





    }
}

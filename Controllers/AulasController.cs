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



    }
}

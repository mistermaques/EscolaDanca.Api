using EscolaDanca.Api.Data;
using EscolaDanca.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EscolaDanca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinanceiroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FinanceiroController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListarPagamentos()
        {
            var pagamentos = await _context.Pagamentos
                .Include(p => p.Aluno)
                .Include(p => p.Aula)
                .OrderByDescending(p => p.DataPagamento)
                .Select(p => new PagamentoDetalhadoDto
                {
                    AlunoName = p.Aluno.Name,
                    AulaNome = p.Aula.Nome,
                    MetodoPagamento = p.MetodoPagamento,
                    Valor = p.Valor,
                    DataPagamento = p.DataPagamento
                })
                .ToListAsync();

            return Ok(pagamentos);
        }


    }

}

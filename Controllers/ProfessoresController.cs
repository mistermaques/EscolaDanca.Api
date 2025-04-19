using EscolaDanca.Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace EscolaDanca.Api.Controllers
{

    
        [ApiController]
        [Route("api/[controller]")]
        public class ProfessoresController : ControllerBase
        {
            private readonly AppDbContext _context;

            public ProfessoresController(AppDbContext context)
            {
                _context = context;
            }

            [HttpGet("listar")]
            public IActionResult Listar()
            {
                var professores = _context.Usuarios
                    .Where(u => u.TipoUsuario == "Professor")
                    .Select(p => new
                    {
                        p.Id,
                        p.Name
                    })
                    .ToList();

                return Ok(professores);
            }
        }

    
}

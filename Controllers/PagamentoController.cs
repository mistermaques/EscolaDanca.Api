using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using EscolaDanca.Api.Data;
using static System.Net.WebRequestMethods;
using EscolaDanca.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EscolaDanca.Api.Controllers
{
    [ApiController]
    [Route("api/pagamento")]
    public class PagamentoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public PagamentoController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("gerar-link")]
        public async Task<IActionResult> GerarLinkPagamento([FromBody] PedidoPagamentoDto dto)
        {
            var accessToken = "APP_USR-1881230836603500-042109-d4727a18b55412fc8e65dd71c2b60140-2401691518";

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var payload = new
            {
                items = new[]
                {
                    new {
                        title = dto.Titulo,
                        quantity = 1,
                        unit_price = dto.Valor
                    }
                },
                external_reference = $"usuario_{dto.UsuarioId}|aula_{dto.AulaId}",
                back_urls = new
                {
                    success = $"https://c937-143-0-191-156.ngrok-free.app/visualizar-aula/{dto.AulaId}?status=sucesso",
                    failure = $"https://c937-143-0-191-156.ngrok-free.app/visualizar-aula/{dto.AulaId}?status=erro",
                    pending = $"https://c937-143-0-191-156.ngrok-free.app/visualizar-aula/{dto.AulaId}?status=pendente"

                },
                auto_return = "approved",
                notification_url = "https://c937-143-0-191-156.ngrok-free.app/api/pagamento/notificacao"

            };
            Console.WriteLine($"Payload Enviado: {JsonSerializer.Serialize(payload)}");

            var response = await client.PostAsJsonAsync("https://api.mercadopago.com/checkout/preferences", payload);

            if (!response.IsSuccessStatusCode)
                return BadRequest("Erro ao gerar link");

            using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            var link = json.RootElement.GetProperty("init_point").GetString();

            return Ok(new { url = link });
        }

        [HttpPost("notificacao")]
        public async Task<IActionResult> Notificacao([FromBody] JsonElement body)
        {
            try
            {
                string paymentId = body.GetProperty("data").GetProperty("id").ToString();

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "APP_USR-1881230836603500-042109-d4727a18b55412fc8e65dd71c2b60140-2401691518");

                var response = await client.GetAsync($"https://api.mercadopago.com/v1/payments/{paymentId}");
                if (!response.IsSuccessStatusCode)
                    return BadRequest("Erro ao consultar pagamento");

                using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
                var pagamento = json.RootElement;

                string status = pagamento.GetProperty("status").GetString() ?? "";
                string externalRef = pagamento.GetProperty("external_reference").GetString() ?? "";

                if (status.ToLower() == "approved")
                {
                    var partes = externalRef.Split('|');
                    int usuarioId = int.Parse(partes[0].Replace("usuario_", ""));
                    int aulaId = int.Parse(partes[1].Replace("aula_", "").Replace("aula_", ""));

                    // Validação: verificar se o usuário e a aula existem no banco antes
                    var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == usuarioId);
                    var aulaExiste = await _context.Aulas.AnyAsync(a => a.Id == aulaId);

                    if (!usuarioExiste || !aulaExiste)
                    {
                        Console.WriteLine($"⚠️ Usuário ID {usuarioId} ou Aula ID {aulaId} não encontrado.");
                        return Ok(); // Retorna OK para o MercadoPago mesmo assim
                    }

                    var jaExiste = await _context.AlunosAulas
                        .AnyAsync(aa => aa.UsuarioId == usuarioId && aa.AulaId == aulaId);

                    if (!jaExiste)
                    {
                        var alunoAula = new AlunoAula
                        {
                            UsuarioId = usuarioId,
                            AulaId = aulaId,
                            DataAssinatura = DateTime.UtcNow
                        };

                        _context.AlunosAulas.Add(alunoAula);
                        await _context.SaveChangesAsync();
                        Console.WriteLine($"Aluno {usuarioId} vinculado à aula {aulaId} com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine($"Aluno {usuarioId} já está vinculado à aula {aulaId}.");
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO na notificação: " + ex.Message);
                return StatusCode(500, $"Erro ao processar notificação: {ex.Message}");
            }
        }
    }
} 

    public class PedidoPagamentoDto
    {
        public int UsuarioId { get; set; }
        public int AulaId { get; set; } // 🔵 trocamos de PlanoId para AulaId
        public string Titulo { get; set; } = "";
        public decimal Valor { get; set; }
    }

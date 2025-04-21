using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using EscolaDanca.Api.Data;

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
                external_reference = $"usuario_{dto.UsuarioId}|plano_{dto.PlanoId}",
                back_urls = new
                {
                    success = "https://seusite.com/pagamento-sucesso",
                    failure = "https://seusite.com/pagamento-falha"
                },
                auto_return = "approved",
                notification_url = "https://15e2-143-0-191-144.ngrok-free.app/api/pagamento/notificacao"

            };

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
                    int planoId = int.Parse(partes[1].Replace("plano_", ""));

                    var usuario = await _context.Usuarios.FindAsync(usuarioId);
                    if (usuario != null)
                    {
                        usuario.PlanoId = planoId;
                        usuario.StatusPagamento = "Pago";
                        usuario.DataAssinatura = DateTime.UtcNow;
                        usuario.ValidadeAssinatura = DateTime.UtcNow.AddMonths(1);

                        await _context.SaveChangesAsync();
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

    public class PedidoPagamentoDto
    {
        public int UsuarioId { get; set; }
        public int PlanoId { get; set; }
        public string Titulo { get; set; } = "";
        public decimal Valor { get; set; }
    }
}

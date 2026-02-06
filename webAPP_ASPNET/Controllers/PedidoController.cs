using Microsoft.AspNetCore.Mvc;
using webAPP_ASPNET.DTOs;
using webAPP_ASPNET.Services;

namespace webAPP_ASPNET.Controllers
{
    public class PedidoController : Controller
    {
        private readonly PedidoService _pedidoService;

        // ✅ INJEÇÃO DE DEPENDÊNCIA
        public PedidoController(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost]
        public async Task<IActionResult> Finalizar(PedidoCreateDto dto)
        {
            var pedidoId = await _pedidoService.CriarPedido(dto);
            return RedirectToAction("Sucesso", new { id = pedidoId });
        }

        public IActionResult Sucesso(int id)
        {
            ViewBag.PedidoId = id;
            return View();
        }

        [HttpGet]
        public IActionResult PedidoConfirmado(int id)
        {
            ViewBag.PedidoId = id;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MeusPedidos()
        {
            var emailCliente = User.Identity.Name; // ou de onde você pega o email do logado
            var client = new HttpClient();
            var response = await client.GetAsync($"https://localhost:44342/api/pedido/meus-pedidos/{emailCliente}");

            if (!response.IsSuccessStatusCode)
                return View(new List<PedidoViewDto>());

            var pedidos = await response.Content.ReadFromJsonAsync<List<PedidoViewDto>>();
            return View(pedidos);
        }
    }
}
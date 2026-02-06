using Microsoft.AspNetCore.Mvc;
using webAPP_ASPNET.DTOs;
using webAPP_ASPNET.Extensions;
using webAPP_ASPNET.Models;
using webAPP_ASPNET.Services;
using YourNamespace.Filters;

namespace webAPP_ASPNET.Controllers
{
    [TokenAuthorize]
    public class CartController : Controller
    {
        private const string CART_SESSION_KEY = "CART";
        private readonly PedidoService _pedidoService;

        public CartController(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_SESSION_KEY)
                       ?? new List<CartItem>();

            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, string name, decimal price, string image)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_SESSION_KEY)
                       ?? new List<CartItem>();

            var item = cart.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    Name = name,
                    Price = price,
                    Quantity = 1,
                    Image = image
                });
            }

            HttpContext.Session.SetObject(CART_SESSION_KEY, cart);

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int productId)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_SESSION_KEY)
                       ?? new List<CartItem>();

            var item = cart.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
                cart.Remove(item);

            HttpContext.Session.SetObject(CART_SESSION_KEY, cart);

            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove(CART_SESSION_KEY);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_SESSION_KEY)
                       ?? new List<CartItem>();

            if (!cart.Any())
                return RedirectToAction("Index");

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarCompra(PedidoViewModel model)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("CART");

            if (cart == null || !cart.Any())
                return RedirectToAction("Index", "Cart");

            var pedidoDto = new PedidoCreateDto
            {
                NomeCliente = model.NomeCliente,
                EmailCliente = model.EmailCliente,
                Cep = model.Cep,
                Rua = model.Rua,
                Numero = model.Numero,
                Complemento = model.Complemento,
                FormaPagamento = model.FormaPagamento,
                Total = cart.Sum(x => x.Price * x.Quantity),
                Itens = cart.Select(x => new PedidoItemDto
                {
                    ProdutoId = x.ProductId,
                    NomeProduto = x.Name,
                    PrecoUnitario = x.Price,
                    Quantidade = x.Quantity
                }).ToList()
            };

            var pedidoId = await _pedidoService.CriarPedido(pedidoDto);

            HttpContext.Session.Remove("CART");

            return RedirectToAction("PedidoConfirmado", "Pedido", new { id = pedidoId });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using webAPP_ASPNET.Models;
using YourNamespace.Filters;
using webAPP_ASPNET.Extensions;

namespace webAPP_ASPNET.Controllers
{
    [TokenAuthorize]
    public class CartController : Controller
    {
        private const string CART_SESSION_KEY = "CART";

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
    }
}
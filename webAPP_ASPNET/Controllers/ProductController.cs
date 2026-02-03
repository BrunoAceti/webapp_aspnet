using Microsoft.AspNetCore.Mvc;
using webAPP_ASPNET.Models;
using YourNamespace.Filters;

namespace webAPP_ASPNET.Controllers
{
    [TokenAuthorize]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            var products = new List<CartItem>
            {
                new CartItem
                {
                    ProductId = 1,
                    Name = "Air Jordan 1 Low Travis",
                    Price = 2799.00m,
                    Image = "/resources/jordan.jpg"
                },
                new CartItem
                {
                    ProductId = 2,
                    Name = "Air Jordan 4 Olive",
                    Price = 2499.00m,
                    Image = "/resources/olive.jpg"
                },
                new CartItem
                {
                    ProductId = 3,
                    Name = "Nike Dunk Low Panda",
                    Price = 1099.00m,
                    Image = "/resources/panda.jpg"
                },
                new CartItem
                {
                    ProductId = 4,
                    Name = "Nike Dunk Low Mummy",
                    Price = 1499.00m,
                    Image = "/resources/mummy.jpg"
                }
            };

            return View(products);
        }
    }
}
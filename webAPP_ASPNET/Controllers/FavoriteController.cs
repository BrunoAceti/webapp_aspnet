using Microsoft.AspNetCore.Mvc;
using webAPP_ASPNET.Models;
using webAPP_ASPNET.Extensions;

namespace webAPP_ASPNET.Controllers
{
    public class FavoriteController : Controller
    {
        private const string FAVORITES_KEY = "FAVORITES";

        public IActionResult Index()
        {
            var favorites = HttpContext.Session
                .GetObject<List<FavoriteItem>>(FAVORITES_KEY)
                ?? new List<FavoriteItem>();

            return View(favorites);
        }

        [HttpPost]
        public IActionResult AddToFavorites(FavoriteItem item)
        {
            var favorites = HttpContext.Session
                .GetObject<List<FavoriteItem>>(FAVORITES_KEY)
                ?? new List<FavoriteItem>();

            if (!favorites.Any(x => x.ProductId == item.ProductId))
            {
                favorites.Add(item);
                HttpContext.Session.SetObject(FAVORITES_KEY, favorites);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Remove(int id)
        {
            var favorites = HttpContext.Session
                .GetObject<List<FavoriteItem>>(FAVORITES_KEY)
                ?? new List<FavoriteItem>();

            var item = favorites.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                favorites.Remove(item);
                HttpContext.Session.SetObject(FAVORITES_KEY, favorites);
            }

            return RedirectToAction("Index");
        }
    }
}
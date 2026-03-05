using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using webAPP_ASPNET.Models;

namespace webAPP_ASPNET.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var loginModel = new
            {
                USERNAME = username,
                PASSWORD = password
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");
            HttpContext.Session.SetString("BaseSelecionada", Data.ApiSettings.ApiBaseURL);

            string baseUrl = HttpContext.Session.GetString("BaseSelecionada");
            if (string.IsNullOrEmpty(baseUrl))
            {
                ModelState.AddModelError(string.Empty, "A URL base não foi configurada.");
                return View("Index");
            }

            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await _httpClient.PostAsync("User/login", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string token = tokenObject?.token;

                if (!string.IsNullOrEmpty(token))
                {
                    HttpContext.Session.SetString("Token", token);

                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response2 =
                        await _httpClient.GetAsync($"User/Information/{username}/{password}");

                    if (response2.IsSuccessStatusCode)
                    {
                        var responseContent2 = await response2.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<UserWithDepartment>(responseContent2);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.User.USERNAME),
                            new Claim(ClaimTypes.Email, user.User.EMAIL),
                            new Claim(ClaimTypes.NameIdentifier, user.User.ID.ToString()),
                            new Claim("FullName", user.User.FULLNAME),
                            new Claim(ClaimTypes.Role, "ADM")
                        };

                        var identity = new ClaimsIdentity(
                            claims,
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            principal
                        );

                        LoggedUser.User.ID = user.User.ID;
                        LoggedUser.User.FULLNAME = user.User.FULLNAME;
                        LoggedUser.User.USERNAME = user.User.USERNAME;
                        LoggedUser.User.EMAIL = user.User.EMAIL;
                        LoggedUser.Department.ID = user.Department.ID;
                        LoggedUser.Department.DEPARTMENTNAME = user.Department.DEPARTMENTNAME;
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Nome de usuário ou senha inválidos.");
            return View("Index");
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string fullname, string email, string username, string password)
        {
            HttpContext.Session.SetString("BaseSelecionada", Data.ApiSettings.ApiBaseURL);

            var registerModel = new
            {
                FULLNAME = fullname,
                EMAIL = email,
                USERNAME = username,
                PASSWORD = password
            };

            var json = new StringContent(
                JsonConvert.SerializeObject(registerModel),
                Encoding.UTF8,
                "application/json"
            );

            string baseUrl = HttpContext.Session.GetString("BaseSelecionada");

            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );

            var response = await _httpClient.PostAsync("User/register", json);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Erro ao criar conta");
            return View();
        }
    }
}

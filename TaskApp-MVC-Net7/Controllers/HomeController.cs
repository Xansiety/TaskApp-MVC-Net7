using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using TaskApp.Models;

namespace TaskApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            this.localizer = localizer;
        }

        public IActionResult Index()
        {
            ViewBag.Saludo = localizer["Buenos días"];
            return View();
        }

        [HttpPost]
        public IActionResult CambiarIdioma(string cultura, string urlRetorna)
        {
            Response.Cookies.Append(
                     CookieRequestCultureProvider.DefaultCookieName,
                     CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cultura)),
                     new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                 );

            return LocalRedirect(urlRetorna);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
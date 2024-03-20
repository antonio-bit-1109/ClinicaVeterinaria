using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ClinicaVeterinaria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier))
            {
                return RedirectToAction("Index", "Prodotti");
            }

            string navfoot = "grad";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            return View();
        }

        public IActionResult Privacy()
        {
            string navfoot = "grad";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            string navfoot = "grad";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

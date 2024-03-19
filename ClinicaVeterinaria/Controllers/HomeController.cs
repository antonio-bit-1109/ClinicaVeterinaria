using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            string navfoot = "grad";
            ViewBag.NavFoot = navfoot;
			string text = "wh";
			ViewBag.Text = text;

            return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

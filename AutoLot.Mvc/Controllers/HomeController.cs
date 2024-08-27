using AutoLot.Mvc.Models;
using AutoLot.Services.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AutoLot.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAppLogging<HomeController> _logger;

        public HomeController(IAppLogging<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogAppInformation("This is a test.");
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

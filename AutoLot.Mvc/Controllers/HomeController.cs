using AutoLot.Mvc.Models;
using AutoLot.Services.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

        public IActionResult Index([FromServices] IOptionsMonitor<DealerInfo> dealerMonitor)
        {
            var vm = dealerMonitor.CurrentValue;
            return View(vm);
        }

        public IActionResult Privacy([FromServices] IOptionsMonitor<CreatorInfo> creatorMonitor)
        {
            var vm = creatorMonitor.CurrentValue;
            return View(vm);
        }
        public IActionResult InProcess()
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

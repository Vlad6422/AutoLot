using AutoLot.Dal.Repos.Interfaces;
using AutoLot.Services.Logging;
using Microsoft.AspNetCore.Mvc;

namespace AutoLot.Mvc.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IAppLogging<OrdersController> _logging;
        private readonly IOrderRepo _repo;
        public OrdersController(IOrderRepo orderRepo, IAppLogging<OrdersController> logging)
        {
            _logging = logging;
            _repo = orderRepo;
        }

        public IActionResult Index()
        {
            return View(_repo.GetOrdersViewModel());
        }
    }
}

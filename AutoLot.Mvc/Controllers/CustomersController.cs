using AutoLot.Dal.Repos.Interfaces;
using AutoLot.Services.Logging;
using Microsoft.AspNetCore.Mvc;

namespace AutoLot.Mvc.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IAppLogging<CustomersController> _logging;
        private readonly ICustomerRepo _repo;
        public CustomersController(ICustomerRepo customerRepo, IAppLogging<CustomersController> logging)
        {
            _logging = logging;
            _repo = customerRepo;
        }

        public IActionResult Index()
        {
            return View(_repo.GetAllIgnoreQueryFilters());
        }
    }
}

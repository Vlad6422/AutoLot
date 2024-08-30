using AutoLot.Models.Entities;
using AutoLot.Services.ApiWrapper;
using AutoLot.Services.Logging;
using Microsoft.AspNetCore.Mvc;

namespace AutoLot.Mvc.Controllers
{
    [Route("[controller]/[action]")]
    public class CustomersController : Controller
    {
        private readonly IApiServiceWrapper _serviceWrapper;
        private readonly IAppLogging<CustomersController> _logging;
        public CustomersController(IApiServiceWrapper serviceWrapper, IAppLogging<CustomersController> logging)
        {
            _serviceWrapper = serviceWrapper;
            _logging = logging;
            //_logging.LogAppError("Test error");
        }
        [Route("/[controller]")]
        [Route("/[controller]/[action]")]
        public async Task<IActionResult> Index()
            => View(await _serviceWrapper.GetCustomersAsync());

        // GET: Cars/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _serviceWrapper.AddCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
        internal async Task<Customer> GetOneCustomerAsync(int? id)
    => !id.HasValue ? null : await _serviceWrapper.GetCustomerAsync(id.Value);

        // GET: Customers/Details/5
        [HttpGet("{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            var customer = await GetOneCustomerAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
        // GET: Customers/Edit/5
        [HttpGet("{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            var customer = await GetOneCustomerAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                await _serviceWrapper.UpdateCustomerAsync(id, customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
        // GET: Customers/Delete/5
        [HttpGet("{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            var car = await GetOneCustomerAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: Customers/Delete/5
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Customer customer)
        {
            await _serviceWrapper.DeleteCustomerAsync(id, customer);
            return RedirectToAction(nameof(Index));
        }

    }
}

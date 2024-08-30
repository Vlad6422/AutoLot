using AutoLot.Dal.Repos.Interfaces;
using AutoLot.Models.Entities;
using AutoLot.Services.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoLot.Mvc.Controllers
{
    [Route("[controller]/[action]")]
    public class CarsDalController : Controller
    {
        private readonly IAppLogging<CarsController> _logging;
        private readonly ICarRepo _repo;
        public CarsDalController(ICarRepo carRepo, IAppLogging<CarsController> logging)
        {
            _logging = logging;
            _repo = carRepo;
        }

        public IActionResult Index()
        {
            return View(_repo.GetAllIgnoreQueryFilters());
        }
        internal SelectList GetMakes(IMakeRepo makeRepo)
            => new SelectList(makeRepo.GetAll(), nameof(Make.Id), nameof(Make.Name));
        [HttpGet]
        public IActionResult Create([FromServices] IMakeRepo makeRepo)
        {
            ViewData["MakeId"] = GetMakes(makeRepo);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromServices] IMakeRepo makeRepo, Car car)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(car);
                return RedirectToAction(nameof(Details), new { id = car.Id });
            }
            return View(car);
        }
        public IActionResult Delete()
        {
            return View();
        }
        [HttpGet("{id?}")]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            var car = GetOneCar(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }
        internal Car GetOneCar(int? id)
            => !id.HasValue ? null : _repo.Find(id.Value);
        [HttpGet("{id?}")]
        public IActionResult Edit([FromServices] IMakeRepo makeRepo, int? id)
        {
            var car = GetOneCar(id);
            if (car == null)
            {
                return NoContent();
            }
            ViewData["MakeId"] = GetMakes(makeRepo);
            return View(car);
        }
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromServices] IMakeRepo makeRepo, int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _repo.Update(car);
                return RedirectToAction(nameof(Details), new { id = car.Id });
            }
            ViewData["MakeId"] = GetMakes(makeRepo);
            return View(car);
        }
        [HttpGet("{id?}")]
        public IActionResult Delete(int? id)
        {
            var car = GetOneCar(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }
            _repo.Delete(car);
            return RedirectToAction(nameof(Index));
        }
    }
}
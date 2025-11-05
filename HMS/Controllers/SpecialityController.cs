using HMS.Interfaces.Services;
using HMS.Models.DTOs.Specialty;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HMS.Controllers
{
    public class SpecialityController : Controller
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialityController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }
        public async Task<IActionResult> Index()
        {
            var specialities = await _specialtyService.GetSpecialitiesAsync(CancellationToken.None);
            return View(specialities);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSpecialtyRequestModel model)
        {
            var specialty = await _specialtyService.CreateAsync(model);

            if (!specialty.Status)
            {
                ViewBag.ErrroMEssage = specialty.Message;
            }

            return RedirectToAction("Index");
        }
    }

}

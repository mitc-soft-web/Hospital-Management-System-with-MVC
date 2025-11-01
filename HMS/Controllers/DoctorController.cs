using HMS.Interfaces.Services;
using HMS.Models.DTOs.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HMS.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly ISpecialtyService _specialtyService;

        public DoctorController(IDoctorService doctorService, ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService ?? throw new ArgumentNullException(nameof(specialtyService));
            _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));

        }
        public async Task<IActionResult> Index()
        {
            var specialities = await _specialtyService.GetSpecialitiesAsync(CancellationToken.None);

            return View(specialities);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var roles = await _specialtyService.GetSpecialitiesAsync(CancellationToken.None);
            ViewData["SpecialityIds"] = new SelectList(roles.Data, "Id", "Name");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateDoctorRequestModel model)
        {
            var doctor = await _doctorService.CreateAsync(model);
            if (!doctor.Status)
            {
                ViewBag.Failed = "Patient creation unsuccessful";
            }
            ViewBag.Success = "Patient created successfully";

            return View(doctor);
        }
    }
}

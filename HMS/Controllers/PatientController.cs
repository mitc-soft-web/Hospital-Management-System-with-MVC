using HMS.Interfaces.Services;
using HMS.Models.DTOs.Patient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace HMS.Controllers
{
    public class PatientController : Controller
    {
        private IPatientService _patientService;
        private IRoleService _roleService;

        public PatientController(IPatientService patientService, IRoleService roleService)
        {
            _patientService = patientService;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var patients = await _patientService.GetPatientsAsync(CancellationToken.None);
            return View(patients);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleService.GetRolesAsync(CancellationToken.None);
            ViewData["RoleIds"] = new SelectList(roles.Data, "Id", "Name");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreatePatientRequestModel model)
        {
            var patient = await _patientService.CreateAsync(model);
            if (!patient.Status)
            {
                ViewBag.Failed = "Patient creation unsuccessful";
            }
            ViewBag.Success = "Patient created successfully";

            return View(patient);
        }
    }
}

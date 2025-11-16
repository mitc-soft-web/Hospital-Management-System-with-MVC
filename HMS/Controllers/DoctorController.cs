using HMS.Implementation.Services;
using HMS.Interfaces.Services;
using HMS.Models.DTOs.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using System.Security.Claims;

namespace HMS.Controllers
{
    public class DoctorController(IDoctorService doctorService, ISpecialtyService specialtyService,
        IRoleService roleService, ILogger<DoctorController> logger, IUserService userService, 
        IAppointmentService appointmentService) : Controller
    {
        private readonly IDoctorService _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
        private readonly ISpecialtyService _specialtyService = specialtyService ?? throw new ArgumentNullException(nameof(specialtyService));
        private readonly IRoleService roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        private readonly ILogger<DoctorController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        private readonly IAppointmentService _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorService.GetDotorsAsync(CancellationToken.None);

            return View(doctors);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var specialities = await _specialtyService.GetSpecialitiesAsync(CancellationToken.None);
            ViewData["Specialities"] = new MultiSelectList(specialities.Data, "Id", "Name");

            var roles = await roleService.GetRolesAsync(CancellationToken.None);
            ViewData["Roles"] = new MultiSelectList(roles.Data, "Id", "Name");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateDoctorRequestModel model)
        {
            var doctor = await _doctorService.CreateAsync(model);
            if (doctor.Status)
            {
                ViewBag.Alert = doctor.Status;
                ViewBag.AlertType = "success";

                return RedirectToAction("Index");
            }
            else
            {

                ViewBag.Alert = doctor.Status;
                ViewBag.AlertType = "danger";
                return View();
            }
               
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> DoctorProfile()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine(User.FindFirstValue(ClaimTypes.GivenName));

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "User");
            }
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return BadRequest("Invalid user ID format.");
            }

            var doctor = await _userService.GetUserProfileByUserId(userId, CancellationToken.None);

            if (doctor == null || !doctor.Status) return NotFound(doctor.Message);

            return View(doctor);




        }

        [HttpGet]
        public async Task<IActionResult> GetMyAppointments()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine(User.FindFirstValue(ClaimTypes.GivenName));

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("DoctorProfile", "Doctor");
            }
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return BadRequest("Invalid user ID format.");
            }
            var appointments = await _appointmentService.GetAllByDoctorIdAsync(userId, CancellationToken.None);

            return View(appointments);
        }
    }
}

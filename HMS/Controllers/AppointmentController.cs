using HMS.Implementation.Services;
using HMS.Interfaces.Services;
using HMS.Models.DTOs.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HMS.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IDoctorService doctorService, IPatientService patientService,
            IAppointmentService appointmentService)
        {
            _doctorService = doctorService;
            _patientService = patientService;
            _appointmentService = appointmentService;
        }
        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAppointmentsAsync(CancellationToken.None);

            return View(appointments);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var doctors = await _doctorService.GetDotorsAsync(CancellationToken.None);
            if (!doctors.Status)
            {
                return View();
            }
            else
            {
                ViewData["Doctors"] = new SelectList(doctors.Data, "Id", "FullName");
            }
            
            var patients = await _patientService.GetPatientsAsync(CancellationToken.None);
            if(!patients.Status)
            {
                return View();
            }
            else
            {
                ViewData["Patients"] = new SelectList(patients.Data, "Id", "FullName");
            }
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateAppointmentRequestModel model)
        {
            var appointment = await _appointmentService.CreateAsync(model);

            if (!appointment.Status)
            {
                ViewBag.Error = "Error while initializing appointment";
                return View(appointment);
            }

            return RedirectToAction("Index");
        }
    }
}

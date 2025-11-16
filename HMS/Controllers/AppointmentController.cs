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

        [HttpGet]
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
            ViewData["Doctors"] = new SelectList(doctors.Data, "Id", "FullName");

            var patients = await _patientService.GetPatientsAsync(CancellationToken.None);
            ViewData["Patients"] = new SelectList(patients.Data, "Id", "FullName");
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

        [HttpGet("ViewAppointmentDetails/{id}")]
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> ViewAppointmentDetails(Guid id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id, CancellationToken.None);

            Console.WriteLine("Medical No: " + appointment.Data.Patient.MedicalRecordNumber);

            //if (!appointment.Status)
            //{
            //    ViewBag.Error = appointment.Status;
            //}

            return View(appointment);
        }

        [HttpGet("Accept/{id}")]
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> Accept(Guid id)
        {
            var appointment = await _appointmentService.AcceptAppointment(id);

            //if (!appointment.Status)
            //{
            //    ViewBag.Error = appointment.Status;
            //}

            return RedirectToAction("ViewAppointmentDetails");
        }
    }
}

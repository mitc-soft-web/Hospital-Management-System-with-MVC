using HMS.Interfaces.Repositories;
using HMS.Interfaces.Services;
using HMS.Models.DTOs;
using HMS.Models.DTOs.Appointment;
using HMS.Models.DTOs.Patient;
using HMS.Models.DTOs.Patients;
using HMS.Models.Entities;

namespace HMS.Implementation.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AppointmentService> _logger;
        public AppointmentService(IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository, IPatientRepository patientRepository,
            IUnitOfWork unitOfWork,
            ILogger<AppointmentService> logger)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> CreateAsync(CreateAppointmentRequestModel request)
        {
            var doctor = await _doctorRepository.Get<Doctor>(d => d.Id == request.DoctorId);
            if(doctor == null)
            {
                return new BaseResponse<bool>
                {
                    Message = "Doctor cannot be found",
                    Status = false
                };
            }

            var patient = await _patientRepository.Get<Patient>(p => p.Id == request.PatientId);
            if (patient == null)
            {
                return new BaseResponse<bool>
                {
                    Message = "Patient cannot be found",
                    Status = false
                };
            }

            var appointnment = new Appointment
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Description = request.Description,
                Location = request.Location,
                AppointmentDate = request.AppointmentDate,
                Title = request.Title,
                AppointmentStatus = Models.Enums.AppointmentStatus.Scheduled,
                DateCreated = DateTime.UtcNow
            };

            var newAppointment = await _appointmentRepository.Add(appointnment);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            if (newAppointment == null)
            {
                _logger.LogError("Appointment couldn't be initialized");
                return new BaseResponse<bool>
                {
                    Message = "Appointment couldn't be initialized",
                    Status = false
                };
            }
            return new BaseResponse<bool>
            {
                Message = "Appointment initialized",
                Status = true
            };

        }

        public Task<BaseResponse<bool>> DeleteAsync(Guid patientId)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IReadOnlyList<AppointmentDto>>> GetAppointmentsAsync(CancellationToken cancellationToken)
        {
            var appointments = await _appointmentRepository.GetAllAppointmentsAndDetails();
            if(!appointments.Any())
            {
                _logger.LogError("No appointments found");
                return new BaseResponse<IReadOnlyList<AppointmentDto>>
                {
                    Message = "No appointments found",
                    Status = false
                };
            }
            _logger.LogInformation("Data fetched successfully");
            return new BaseResponse<IReadOnlyList<AppointmentDto>>
            {
                Message = "Data fetched successfully",
                Status = true,
                Data = appointments.Select(ap => new AppointmentDto
                {
                    Id = ap.Id,
                    AppointmentDate = ap.AppointmentDate,
                    Description = ap.Description,
                    Location = ap.Location,
                    AppointmentStatus = ap.AppointmentStatus,
                    Title = ap.Title,
                    DateCreated = ap.DateCreated,
                    DoctorId = ap.DoctorId,
                    Doctor = ap.Doctor,
                    PatientId = ap.PatientId,
                    ChangeInAppointmentDescription = ap.ChangeInAppointmentDescription,

                }).ToList()

            };
        }


        public Task<BaseResponse<AppointmentDto>> GetByIdAsync(Guid appointmentId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IReadOnlyList<AppointmentDto>>> GetCancelledAppointmentsAsync(CancellationToken cancellationToken)
        {
            var appointments = await _appointmentRepository.GetAllCancelledAppointments();
            if (!appointments.Any())
            {
                _logger.LogError("No appointments found");
                return new BaseResponse<IReadOnlyList<AppointmentDto>>
                {
                    Message = "No appointments found",
                    Status = false
                };
            }
            _logger.LogInformation("Data fetched successfully");
            return new BaseResponse<IReadOnlyList<AppointmentDto>>
            {
                Message = "Data fetched successfully",
                Status = true,
                Data = appointments.Select(ap => new AppointmentDto
                {
                    Id = ap.Id,
                    AppointmentDate = ap.AppointmentDate,
                    Description = ap.Description,
                    Location = ap.Location,
                    AppointmentStatus = ap.AppointmentStatus,
                    Title = ap.Title,
                    DateCreated = ap.DateCreated,
                    DoctorId = ap.DoctorId,
                    Doctor = ap.Doctor,
                    PatientId = ap.PatientId,
                    ChangeInAppointmentDescription = ap.ChangeInAppointmentDescription,

                }).ToList()

            }; 
        }

        public async Task<BaseResponse<IReadOnlyList<AppointmentDto>>> GetCompletedAppointmentsAsync(CancellationToken cancellationToken)
        {
            var appointments = await _appointmentRepository.GetAllCompletedAppointments();
            if (!appointments.Any())
            {
                _logger.LogError("No appointments found");
                return new BaseResponse<IReadOnlyList<AppointmentDto>>
                {
                    Message = "No appointments found",
                    Status = false
                };
            }
            _logger.LogInformation("Data fetched successfully");
            return new BaseResponse<IReadOnlyList<AppointmentDto>>
            {
                Message = "Data fetched successfully",
                Status = true,
                Data = appointments.Select(ap => new AppointmentDto
                {
                    Id = ap.Id,
                    AppointmentDate = ap.AppointmentDate,
                    Description = ap.Description,
                    Location = ap.Location,
                    AppointmentStatus = ap.AppointmentStatus,
                    Title = ap.Title,
                    DateCreated = ap.DateCreated,
                    DoctorId = ap.DoctorId,
                    Doctor = ap.Doctor,
                    PatientId = ap.PatientId,
                    ChangeInAppointmentDescription = ap.ChangeInAppointmentDescription,

                }).ToList()
            };
        }

        public async Task<BaseResponse<IReadOnlyList<AppointmentDto>>> GetRescheduledAppointmentsAsync(CancellationToken cancellationToken)
        {
            var appointments = await _appointmentRepository.GetAllRescheduledAppointments();
            if (!appointments.Any())
            {
                _logger.LogError("No appointments found");
                return new BaseResponse<IReadOnlyList<AppointmentDto>>
                {
                    Message = "No appointments found",
                    Status = false
                };
            }
            _logger.LogInformation("Data fetched successfully");
            return new BaseResponse<IReadOnlyList<AppointmentDto>>
            {
                Message = "Data fetched successfully",
                Status = true,
                Data = appointments.Select(ap => new AppointmentDto
                {
                    Id = ap.Id,
                    AppointmentDate = ap.AppointmentDate,
                    Description = ap.Description,
                    Location = ap.Location,
                    AppointmentStatus = ap.AppointmentStatus,
                    Title = ap.Title,
                    DateCreated = ap.DateCreated,
                    DoctorId = ap.DoctorId,
                    Doctor = ap.Doctor,
                    PatientId = ap.PatientId,
                    ChangeInAppointmentDescription = ap.ChangeInAppointmentDescription,

                }).ToList()
            };
        }

        public async Task<BaseResponse<IReadOnlyList<AppointmentDto>>> GetScheduledAppointmentsAsync(CancellationToken cancellationToken)
        {
            var appointments = await _appointmentRepository.GetAllRescheduledAppointments();
            if (!appointments.Any())
            {
                _logger.LogError("No appointments found");
                return new BaseResponse<IReadOnlyList<AppointmentDto>>
                {
                    Message = "No appointments found",
                    Status = false
                };
            }
            _logger.LogInformation("Data fetched successfully");
            return new BaseResponse<IReadOnlyList<AppointmentDto>>
            {
                Message = "Data fetched successfully",
                Status = true,
                Data = appointments.Select(ap => new AppointmentDto
                {
                    Id = ap.Id,
                    AppointmentDate = ap.AppointmentDate,
                    Description = ap.Description,
                    Location = ap.Location,
                    AppointmentStatus = ap.AppointmentStatus,
                    Title = ap.Title,
                    DateCreated = ap.DateCreated,
                    DoctorId = ap.DoctorId,
                    Doctor = ap.Doctor,
                    PatientId = ap.PatientId,
                    ChangeInAppointmentDescription = ap.ChangeInAppointmentDescription,

                }).ToList()
            };
        }
    }
}

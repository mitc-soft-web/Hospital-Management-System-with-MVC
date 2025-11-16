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

        public async Task<BaseResponse<bool>> AcceptAppointment(Guid id)
        {
            var appointment = await _appointmentRepository.Get<Appointment>(ap => ap.Id == id);
            if (appointment == null)
            {
                _logger.LogError($"Appointment with Id: '{appointment?.Id}' cannot be found");
                return new BaseResponse<bool>
                {
                    Message = $"Appointment with Id: '{appointment?.Id}' cannot be found",
                    Status = false
                };
            }

            appointment.AppointmentStatus = Models.Enums.AppointmentStatus.Accepted;
            await _unitOfWork.SaveChangesAsync();
            var acceptAppointment = _appointmentRepository.AcceptAppointment(appointment);

            if (!acceptAppointment)
            {
                return new BaseResponse<bool>
                {
                    Message = "Appointment couldn't be accepted",
                    Status = false

                };
            }
            return new BaseResponse<bool>
            {
                Message = "Appointment accepted",
                Status = true
            };

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

            var appointment = new Appointment
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Description = request.Description,
                Location = request.Location,
                AppointmentDate = request.AppointmentDate,
                Title = request.Title,
                AppointmentStatus = Models.Enums.AppointmentStatus.Pending,
                DateCreated = DateTime.UtcNow
            };

            var newAppointment = await _appointmentRepository.Add(appointment);
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

        public async Task<BaseResponse<IReadOnlyList<AppointmentDto>>> GetAllByDoctorIdAsync(Guid userDoctorId, CancellationToken cancellationToken)
        {
            var appointments = await _appointmentRepository.GetAppointmentByDctorId(userDoctorId);
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
                    Title = ap.Title,
                    DateCreated = ap.DateCreated,
                    AppointmentStatus = ap.AppointmentStatus,
                    ChangeInAppointmentDescription = ap.ChangeInAppointmentDescription,
                    Doctor = new Models.DTOs.Doctor.DoctorDto
                    {
                        FullName = ap.Doctor.FullName(),
                        Email = ap.Doctor.User.Email,
                        PhoneNumber = ap.Doctor.PhoneNumber,
                        DoctorSpecialities = ap.Doctor.DoctorSpecialities.Select(sd => sd.Speciality.Name).ToList()

                    },
                    Patient = new PatientDto
                    {
                        FullName = ap.Patient.FullName(),
                        Address = ap.Patient.Address,
                        PhoneNumber = ap.Patient.PhoneNumber,
                        Email = ap.Patient.User.Email,
                        MedicalRecordNumber = ap.Patient.PhoneNumber,
                        PatientDetail = new PatientDetailsDto
                        {
                            Allergies = ap.Patient.PatientDetail.Allergies,
                            MedicalHistory = ap.Patient.PatientDetail.MedicalHistory,
                            Genotype = ap.Patient.PatientDetail.Genotype,
                            EmergencyContact = ap.Patient.PatientDetail.EmergencyContact,
                            BloodGroup = ap.Patient.PatientDetail.BloodGroup
                            
                        }

                    }
                    
                }).ToList()



            };

        }

        public async Task<BaseResponse<IReadOnlyList<AppointmentDto>>> GetAppointmentsAsync(CancellationToken cancellationToken)
        {
            var appointments = await _appointmentRepository.GetAllAppointmentsAndDetails();
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
                    Title = ap.Title,
                    DateCreated = ap.DateCreated,
                    ChangeInAppointmentDescription = ap.ChangeInAppointmentDescription,
                    Doctor = new Models.DTOs.Doctor.DoctorDto
                    {
                        FullName = ap.Doctor.FullName(),
                        Email = ap.Doctor.User.Email,
                        PhoneNumber = ap.Doctor.PhoneNumber,
                        DoctorSpecialities = ap.Doctor.DoctorSpecialities.Select(sd => sd.Speciality.Name).ToList()

                    },
                    Patient = new PatientDto
                    {
                        FullName = ap.Patient.FullName(),
                        Address = ap.Patient.Address,
                        PhoneNumber = ap.Patient.PhoneNumber,
                        Email = ap.Patient.User.Email,

                    }
                }).ToList()

            };
        }
        public async Task<BaseResponse<AppointmentDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(id);
            if (appointment == null)
            {
                _logger.LogError($"Appointment with Id: '{appointment?.Id}' cannot be found");
                return new BaseResponse<AppointmentDto>
                {
                    Message = $"Appointment with Id: '{appointment?.Id}' cannot be found",
                    Status = false
                };
            }

            return new BaseResponse<AppointmentDto>
            {
                Message = $"Appointment with Id: '{id}' found",
                Status = true,
                Data = new AppointmentDto
                {
                    Id = appointment.Id,
                    AppointmentDate = appointment.AppointmentDate,
                    Description = appointment.Description,
                    Location = appointment.Location,
                    AppointmentStatus = appointment.AppointmentStatus,
                    Title = appointment.Title,
                    DateCreated = appointment.DateCreated,
                    ChangeInAppointmentDescription = appointment.ChangeInAppointmentDescription,
                    Doctor = new Models.DTOs.Doctor.DoctorDto
                    {
                        FullName = appointment.Doctor.FullName(),
                        Email = appointment.Doctor.User.Email,
                        PhoneNumber = appointment.Doctor.PhoneNumber,
                        DoctorSpecialities = appointment.Doctor.DoctorSpecialities.Select(sd => sd.Speciality.Name).ToList()

                    },
                    Patient = new PatientDto
                    {
                        FullName = appointment.Patient.FullName(),
                        Address = appointment.Patient.Address,
                        PhoneNumber = appointment.Patient.PhoneNumber,
                        Email = appointment.Patient.User.Email,
                        MedicalRecordNumber = appointment.Patient.MedicalRecordNumber,
                        PatientDetail = new PatientDetailsDto
                        {
                            Allergies = appointment.Patient.PatientDetail.Allergies,
                            MedicalHistory = appointment.Patient.PatientDetail.MedicalHistory,
                            Genotype = appointment.Patient.PatientDetail.Genotype,
                            EmergencyContact = appointment.Patient.PatientDetail.EmergencyContact,
                            BloodGroup = appointment.Patient.PatientDetail.BloodGroup

                        }


                    }
                }
            };
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
                    PatientId = ap.PatientId,
                    ChangeInAppointmentDescription = ap.ChangeInAppointmentDescription,
                    Doctor = new Models.DTOs.Doctor.DoctorDto
                    {
                        FullName = ap.Doctor.FullName(),
                        Email = ap.Doctor.User.Email,
                        PhoneNumber = ap.Doctor.PhoneNumber,
                        DoctorSpecialities = ap.Doctor.DoctorSpecialities.Select(sd => sd.Speciality.Name).ToList()

                    },
                    Patient = new PatientDto
                    {
                        FullName = ap.Patient.FullName(),
                        Address = ap.Patient.Address,
                        PhoneNumber = ap.Patient.PhoneNumber,
                        Email = ap.Patient.User.Email,

                    }

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
                    PatientId = ap.PatientId,
                    ChangeInAppointmentDescription = ap.ChangeInAppointmentDescription,
                    Doctor = new Models.DTOs.Doctor.DoctorDto
                    {
                        FullName = ap.Doctor.FullName(),
                        Email = ap.Doctor.User.Email,
                        PhoneNumber = ap.Doctor.PhoneNumber,
                        DoctorSpecialities = ap.Doctor.DoctorSpecialities.Select(sd => sd.Speciality.Name).ToList()

                    },
                    Patient = new PatientDto
                    {
                        FullName = ap.Patient.FullName(),
                        Address = ap.Patient.Address,
                        PhoneNumber = ap.Patient.PhoneNumber,
                        Email = ap.Patient.User.Email,

                    }

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
                    PatientId = ap.PatientId,
                    ChangeInAppointmentDescription = ap.ChangeInAppointmentDescription,
                    Doctor = new Models.DTOs.Doctor.DoctorDto
                    {
                        FullName = ap.Doctor.FullName(),
                        Email = ap.Doctor.User.Email,
                        PhoneNumber = ap.Doctor.PhoneNumber,
                        DoctorSpecialities = ap.Doctor.DoctorSpecialities.Select(sd => sd.Speciality.Name).ToList()

                    },
                    Patient = new PatientDto
                    {
                        FullName = ap.Patient.FullName(),
                        Address = ap.Patient.Address,
                        PhoneNumber = ap.Patient.PhoneNumber,
                        Email = ap.Patient.User.Email,

                    }

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
                    PatientId = ap.PatientId,
                    ChangeInAppointmentDescription = ap.ChangeInAppointmentDescription,
                    Doctor = new Models.DTOs.Doctor.DoctorDto
                    {
                        FullName = ap.Doctor.FullName(),
                        Email = ap.Doctor.User.Email,
                        PhoneNumber = ap.Doctor.PhoneNumber,
                        DoctorSpecialities = ap.Doctor.DoctorSpecialities.Select(sd => sd.Speciality.Name).ToList()

                    },
                    Patient = new PatientDto
                    {
                        FullName = ap.Patient.FullName(),
                        Address = ap.Patient.Address,
                        PhoneNumber = ap.Patient.PhoneNumber,
                        Email = ap.Patient.User.Email,

                    }

                }).ToList()
            };
        }
    }
}

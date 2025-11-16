using HMS.Interfaces.Repositories;
using HMS.Interfaces.Services;
using HMS.Models.DTOs;
using HMS.Models.DTOs.Patients;
using HMS.Models.DTOs.Role;
using HMS.Models.DTOs.Users;
using HMS.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace HMS.Implementation.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly IDoctorRepository _doctorRepository;

        public UserService(IUserRepository userRepository,
            UserManager<User> userManager, 
            ILogger<UserService> logger,
            IDoctorRepository doctorRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _logger = logger;
            _doctorRepository = doctorRepository;
        }

        public async Task<BaseResponse<int>> GetAllHospitalStaffs(CancellationToken cancellationToken)
        {
            var doctorCounts = await _doctorRepository.GetDoctorCounts();
            if(doctorCounts < 1)
            {
                return new BaseResponse<int>()
                {
                    Message = "No staff found",
                    Status = false,
                };
            }
            return new BaseResponse<int>()
            {
                Message = $"{doctorCounts} staff found",
                Status = true,
                Data = doctorCounts
            };

        }

        public Task<BaseResponse<UserDto>> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<UserDto>> GetUserProfileByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserProfile(userId);
            if (user == null)
            {
                return new BaseResponse<UserDto>
                {
                    Message = "User not found",
                    Status = false
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? string.Empty;

            if (role == "Patient")
            {
                return new BaseResponse<UserDto>
                {
                    Message = "Patient profile fetched",
                    Status = true,
                    Data = new UserDto
                    {

                        Id = user.Id,
                        Email = user.Email,
                        Roles = roles.Select(r => new RoleDto { Name = r }).ToList(),
                        Patient = new Models.DTOs.Patients.PatientDto
                        {
                            Id = user.Patient.Id,
                            FirstName = user.Patient.FirstName,
                            FullName = $"{user.Patient.FirstName} {user.Patient.LastName}",
                            Gender = user.Patient.Gender,
                            MedicalRecordNumber = user.Patient.MedicalRecordNumber,
                            PhoneNumber = user.Patient.PhoneNumber,
                            DateOfBirth = user.Patient.DateOfBirth,
                            Address = user.Patient.Address,
                            PatientDetail = new PatientDetailsDto
                            {
                                Genotype = user.Patient.PatientDetail.Genotype,
                                MedicalHistory = user.Patient.PatientDetail.MedicalHistory,
                                BloodGroup = user.Patient.PatientDetail.BloodGroup,
                            }
                        }
                       




                    }
                };
            }
            if (role == "Doctor")
            {
                return new BaseResponse<UserDto>
                {
                    Message = "Doctor profile fetched",
                    Status = true,

                    Data = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Roles = roles.Select(r => new RoleDto { Name = r }).ToList(),
                        Doctor = new Models.DTOs.Doctor.DoctorDto
                        {
                            FirstName = user.Doctor.FirstName,
                            FullName = $"{user.Doctor.FirstName} {user.Doctor.LastName}",
                            Gender = user.Doctor.Gender,
                            PhoneNumber= user.Doctor.PhoneNumber,
                            Position = user.Doctor.Position,
                            Qualification = user.Doctor.Qualification,
                            YearsOfExperience = user.Doctor.YearsOfExperience,
                            DateOfBirth = user.Doctor.DateOfBirth,
                            Address = user.Doctor.Address,
                            DoctorSpecialities = user.Doctor.DoctorSpecialities.Select(d => d.Speciality.Name).ToList()
                        }
                       


                    }
                };
            }
            return new BaseResponse<UserDto>
            {
                Message = "Admin profile fetched",
                Status = true,
                Data = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = role.Select(r => new RoleDto { Name = role }).ToList(),
                    Admin = new AdminDto
                    {
                        FullName = $"{user.Admin.FirstName} {user.Admin.LastName}"
                    }

                }
            };


        }

        public async Task<BaseResponse<LoginResponseModel>> LoginAsync(LoginRequestModel request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null)
            {
                _logger.LogError("Invalid credentials");
                return new BaseResponse<LoginResponseModel>
                {
                    Message = "Invalid credentials",
                    Status = false
                };
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                _logger.LogError("Invalid credentials");
                return new BaseResponse<LoginResponseModel>
                {
                    Message = "Invalid credentials",
                    Status = false
                };
            }
            var roles = await _userManager.GetRolesAsync(user);

            var role = roles.FirstOrDefault() ?? string.Empty;

            if (role == "Patient")
            {
                return new BaseResponse<LoginResponseModel>
                {
                    Message = "Login successful",
                    Status = true,
                    Data = new LoginResponseModel
                    {
                        
                            UserId = user.Id,
                            Email = user.Email,
                            Roles = roles.Select(r => new RoleDto { Name = r }).ToList(),
                            FirstName = user.Patient != null ?  $"{user.Patient.FirstName}" : string.Empty,
                            FullName = user.Admin != null ? $"{user.Patient?.FullName()}" : string.Empty,




                    }
                };
            }
            if (role == "Doctor")
            {
                return new BaseResponse<LoginResponseModel>
                {
                    Message = "Login successful",
                    Status = true,
                   
                        Data = new LoginResponseModel
                        {
                            UserId = user.Id,
                            Email = user.Email,
                            Roles = roles.Select(r => new RoleDto { Name = r }).ToList(),
                            FirstName = user.Doctor != null ?  $"{user.Doctor.FirstName}" : string.Empty,
                            FullName = user.Admin != null ? $"{user.Doctor?.FullName()}" : string.Empty,


                        }
                };
            }
            return new BaseResponse<LoginResponseModel>
            {
                Message = "Login successful",
                Status = true,
                Data = new LoginResponseModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = role.Select(r => new RoleDto { Name = role }).ToList(),
                    FirstName = user.Admin != null ?  $"{user.Admin.FirstName}" : string.Empty,
                    FullName = user.Admin != null ?  $"{user.Admin.FullName()}" : string.Empty,

                }
            };
        }
    }
}
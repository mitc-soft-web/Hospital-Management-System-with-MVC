using HMS.Contracts.Services;
using HMS.Interfaces.Repositories;
using HMS.Interfaces.Services;
using HMS.Models.DTOs;
using HMS.Models.DTOs.Doctor;
using HMS.Models.DTOs.Specialty;
using HMS.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HMS.Implementation.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IIdentityService _identityService;
        private readonly IRoleRepository _roleRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DoctorService> _logger;
        public DoctorService(IUserRepository userRepository,
            UserManager<User> userManager,
            IIdentityService identityService,
            IRoleRepository roleRepository,
            IDoctorRepository doctorRepository,
            IUnitOfWork unitOfWork,
            ILogger<DoctorService> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _identityService = identityService;
            _roleRepository = roleRepository;
            _doctorRepository = doctorRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<BaseResponse<bool>> CreateAsync(CreateDoctorRequestModel request)
        {
            var doctorExists = await _userRepository.Any(u => u.Email == request.Email);
            if (doctorExists)
            {
                _logger.LogError("Doctor with email already exist");
                return new BaseResponse<bool>
                {
                    Message = "Doctor with email already exist",
                    Status = false
                };
            }

            if (request.PasswordHash != request.ConfirmPassword) return new BaseResponse<bool>
            {
                Message = "Password doesnt match!",
                Status = false,
            };

            (var passwordResult, var message) = ValidatePassword(request.PasswordHash);
            if (!passwordResult) return new BaseResponse<bool> { Message = message, Status = false };


            var doctorUser = new User
            {
                Email = request.Email,
                PasswordHash = _identityService.GetPasswordHash(request.PasswordHash)
            };

            var strategy = _unitOfWork.CreateExecutionStrategy();

            BaseResponse<bool> response = await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    
                    var newUserResult = await _userManager.CreateAsync(doctorUser);
                    if (!newUserResult.Succeeded)
                    {
                        throw new Exception("User creation failed: " + string.Join(", ", newUserResult.Errors.Select(e => e.Description)));
                    }

                    
                    var roles = await _roleRepository.GetRolesByIdsAsync(r => request.RoleIds.Contains(r.Id));
                    var roleNames = roles.Select(r => r.Name).ToList();
                    var roleResult = await _userManager.AddToRolesAsync(doctorUser, roleNames);
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception("Adding roles failed: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    }

                
                    var doctor = new Doctor
                    {
                        UserId = doctorUser.Id, 
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Address = request.Address,
                        Gender = request.Gender,
                        PhoneNumber = request.PhoneNumber,
                        Position = request.Position,
                        Qualification = request.Qualification,
                        YearsOfExperience = request.YearsOfExperience,
                        DateCreated = DateTime.UtcNow
                    };

                    foreach (var specialityId in request.SpecialityIds)
                    {
                        var doctorSpeciality = new DoctorSpeciality
                        {
                            SpecialityId = specialityId,
                        };

                        doctor.DoctorSpecialities.Add(doctorSpeciality);
                    }

                    await _doctorRepository.Add(doctor);
                    await _unitOfWork.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return new BaseResponse<bool>
                    {
                        Message = "Doctor created successfully",
                        Status = true
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error creating doctor");
                    return new BaseResponse<bool>
                    {
                        Message = "An error occurred while creating doctor: " + ex.Message,
                        Status = false
                    };
                }
            });

            return response;

        }

        public Task<BaseResponse<bool>> DeleteAsync(Guid doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<DoctorDto>> GetAsync(string param, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<DoctorDto>> GetByIdAsync(Guid dotorId, CancellationToken cancellationToken)
        {
            var doctor = await _doctorRepository.GetDoctorByIdAsync(dotorId);
            if (doctor == null)
            {
                _logger.LogError("Doctor doesn't exist");
                return new BaseResponse<DoctorDto>
                {
                    Message = "Doctor doesn't exist",
                    Status = false
                };
            }

            _logger.LogInformation("Doctor fetched successfully");
            return new BaseResponse<DoctorDto>
            {
                Message = "Doctor fetched successfully",
                Status = true,
                Data = new DoctorDto
                {
                    Id = doctor.Id,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    FullName = doctor.FullName(),
                    Address = doctor.Address,
                    DateOfBirth = doctor.DateOfBirth,
                    Email = doctor.User.Email,
                    DoctorSpecialities = doctor.DoctorSpecialities.Select(ds => ds.Speciality.Name).ToList(),
                    PhoneNumber = doctor.PhoneNumber,
                    Position = doctor.Position,
                    YearsOfExperience = doctor.YearsOfExperience,
                    Qualification = doctor.Qualification,
                    DateCreated = doctor.DateCreated,

                }
            };
        }

        public async Task<BaseResponse<IReadOnlyList<DoctorDto>>> GetDotorsAsync(CancellationToken cancellationToken)
        {
            var doctors = await _doctorRepository.GetAllDoctorsAndTheirSpecialities();
            if (!doctors.Any())
            {
                _logger.LogError("No data found");
                return new BaseResponse<IReadOnlyList<DoctorDto>>
                {
                    Message = "No data found",
                    Status = false
                };
            }

            return new BaseResponse<IReadOnlyList<DoctorDto>>
            {
                Message = "Date fetched seuccessfully",
                Status = true,
                Data = doctors.Select(d => new DoctorDto
                {
                    Id = d.Id,
                    FullName = d.FullName(),
                    Gender = d.Gender,
                    Position = d.Position,
                    YearsOfExperience = d.YearsOfExperience,
                    DoctorSpecialities = d.DoctorSpecialities.Select(sd => sd.Speciality.Name).ToList()


                }).ToList()
            };
        }

        private static (bool, string?) ValidatePassword(string password)
        {
            // Minimum length of password
            int minLength = 8;

            // Maximum length of password
            int maxLength = 50;

            // Check for null or empty password
            if (string.IsNullOrEmpty(password))
            {
                return (false, "Password cannot be null or empty.");
            }

            // Check length of password
            if (password.Length < minLength || password.Length > maxLength)
            {
                return (false, $"Password must be between {minLength} and {maxLength} characters long.");
            }

            // Check for at least one uppercase letter, one lowercase letter, and one digit
            bool hasUppercase = false;
            bool hasLowercase = false;
            bool hasDigit = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    hasUppercase = true;
                }
                else if (char.IsLower(c))
                {
                    hasLowercase = true;
                }
                else if (char.IsDigit(c))
                {
                    hasDigit = true;
                }
            }

            if (!hasUppercase || !hasLowercase || !hasDigit)
            {
                return (false, "Password must contain at least one uppercase letter, one lowercase letter, and one digit.");
            }

            // Check for any characters
            string invalidCharacters = @" !""#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
            if (password.IndexOfAny(invalidCharacters.ToCharArray()) == -1)
            {
                return (false, "Password must contain one or more characters.");
            }

            // Password is valid
            return (true, null);
        }
    }
}

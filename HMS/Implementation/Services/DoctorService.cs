using HMS.Contracts.Services;
using HMS.Interfaces.Repositories;
using HMS.Interfaces.Services;
using HMS.Models.DTOs;
using HMS.Models.DTOs.Doctor;
using HMS.Models.Entities;
using Microsoft.AspNetCore.Identity;

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
        ILogger<DoctorService> _logger;
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

            var doctorUser = new User
            {
                Email = request.Email,
            };

            if (request.PasswordHash != request.ConfirmPassword) return new BaseResponse<bool>
            {
                Message = "Password doesnt match!",
                Status = false,
            };

            (var passwordResult, var message) = ValidatePassword(request.PasswordHash);
            if (!passwordResult) return new BaseResponse<bool> { Message = message, Status = false };

            doctorUser.PasswordHash = _identityService.GetPasswordHash(request.PasswordHash);

            var newUser = await _userManager.CreateAsync(doctorUser);
            if (newUser == null)
            {
                _logger.LogError("User Creation unsuccessful");
                return new BaseResponse<bool>
                {
                    Message = "User Creation unsuccessful",
                    Status = false
                };

            }
            var roles = await _roleRepository.GetRolesByIdsAsync(r => request.RoleIds.Contains(r.Id));

            var userRoleNames = roles.Select(r => r.Name).ToList();

            var result = await _userManager.AddToRolesAsync(doctorUser, userRoleNames);
            if (!result.Succeeded)
            {
                _logger.LogError("Unable to add user to roles");
                return new BaseResponse<bool>
                {
                    Message = "Unable to add user to roles",
                    Status = false
                };
            }

            var userRoles = await _userManager.GetRolesAsync(doctorUser);

            var token = _identityService.GenerateToken(doctorUser, userRoles);

            if (!result.Succeeded)
            {
                throw new Exception($"Unable to add doctor to roles");
            }

            var doctor = new Doctor
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Qualification = request.Qualification,
                YearsOfExperience = request.YearsOfExperience

            };
            foreach(var speciality in request.SpecialityIds)
            {
                doctor.DoctorSpecialities.Add(new DoctorSpeciality
                {
                    SpecialityId = speciality,


                });
            }

            var createDoctor = await _doctorRepository.Add(doctor);
            if(createDoctor == null)
            {
                _logger.LogError("Couldn't create doctor");
                return new BaseResponse<bool>
                {
                    Message = "Couldn't create doctor",
                    Status = false
                };
            }
            _logger.LogInformation("Doctor created successfully");
            return new BaseResponse<bool>
            {
                Message = "Doctor created successfully",
                Status = true
            };

        }

        public Task<BaseResponse<bool>> DeleteAsync(Guid doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<DoctorDto>> GetAsync(string param, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<DoctorDto>> GetByIdAsync(Guid dotorId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IReadOnlyList<DoctorDto>>> GetPatientsAsync(CancellationToken cancellationToken)
        {
            var doctors = await _doctorRepository.GetAll<Doctor>();
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
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Address = d.Address,
                    DateOfBirth = d.DateOfBirth,
                    Email = d.User.Email,
                    Gender = d.Gender,
                    Qualification = d.Qualification,
                    YearsOfExperience = d.YearsOfExperience,
                    DateModified = d.DateModified,


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

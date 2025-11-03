using HMS.Contracts.Services;
using HMS.Interfaces.Repositories;
using HMS.Interfaces.Services;
using HMS.Models.DTOs;
using HMS.Models.DTOs.Patient;
using HMS.Models.DTOs.Patients;
using HMS.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace HMS.Implementation.Services
{

    public class PatientService : IPatientService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IIdentityService _identityService;
        private readonly IRoleRepository _roleRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IUnitOfWork _unitOfWork;
        ILogger<PatientService> _logger;
        public PatientService(IUserRepository userRepository,
            UserManager<User> userManager,
            IIdentityService identityService,
            IRoleRepository roleRepository,
            IPatientRepository patientRepository,
            IUnitOfWork unitOfWork,
            ILogger<PatientService> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _identityService = identityService;
            _roleRepository = roleRepository;
            _patientRepository = patientRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<BaseResponse<bool>> CreateAsync(CreatePatientRequestModel request)
        {

            var patientExists = await _userRepository.Any(u => u.Email == request.Email);
            if (patientExists)
            {
                _logger.LogError("Patient with email already exist");
                return new BaseResponse<bool>
                {
                    Message = "Patient with email already exist",
                    Status = false
                };
            }

            var patientUser = new User
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

            patientUser.PasswordHash = _identityService.GetPasswordHash(request.PasswordHash);

            var newUser = await _userManager.CreateAsync(patientUser);
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

            var result = await _userManager.AddToRolesAsync(patientUser, userRoleNames);
            if (!result.Succeeded)
            {
                _logger.LogError("Unable to add user to roles");
                return new BaseResponse<bool>
                {
                    Message = "Unable to add user to roles",
                    Status = false
                };
            }

            var userRoles = await _userManager.GetRolesAsync(patientUser);

            //var token = _identityService.GenerateToken(patientUser, userRoles);

            if (!result.Succeeded)
            {
                throw new Exception($"Unable to add patient to roles");
            }

            var patient = new Patient
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                PhoneNumber = request.PhoneNumber,
                MedicalRecordNumber = GeneratePatientMedicalRecordNumber(request.FirstName, request.LastName),
                UserId = patientUser.Id,
                DateCreated = DateTime.UtcNow,
                PatientDetail = new PatientDetail
                {
                    Allergies = request.Allergies,
                    BloodGroup = request.BloodGroup,
                    EmergencyContact = request.EmergencyContact,
                    Genotype = request.Genotype,
                    MedicalHistory = request.MedicalHistory,
                    DateCreated = DateTime.UtcNow,
                }


            };

            patient.FullName();
            var createPatient = await _patientRepository.Add(patient);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            if (createPatient == null)
            {
                _logger.LogError("Patient couldn't be added");
                return new BaseResponse<bool>
                {
                    Message = "Patient couldn't be added",
                    Status = false,
                };
            }
            _logger.LogInformation("Patient added successfully");
            return new BaseResponse<bool>
            {
                Message = "Patient added successfully",
                Status = true
            };
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Guid patientId)
        {
            var getPatient = await _patientRepository.Get<Patient>(p => p.Id == patientId);
            if (getPatient == null)
            {
                _logger.LogError("Patient couldn't be found");
                return new BaseResponse<bool>
                {
                    Message = "Patient couldn't be found",
                    Status = false
                };
            }
            _patientRepository.Delete<Patient>(getPatient);
            return new BaseResponse<bool>
            {
                Message = "Patient deletion successful",
                Status = true
            };
        }

        public Task<IReadOnlyList<PatientDto>> GetAsync(string param, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<PatientDto>> GetByIdAsync(Guid patientId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IReadOnlyList<PatientDto>>> GetPatientsAsync(CancellationToken cancellationToken)
        {
            var patients = await _patientRepository.GetAll<Patient>();

            if (!patients.Any())
            {
                _logger.LogError("No data found");
                return new BaseResponse<IReadOnlyList<PatientDto>>
                {
                    Message = "No data found",
                    Status = false
                };
            }

            return new BaseResponse<IReadOnlyList<PatientDto>>
            {
                Message = "Data fetched successfully",
                Data = patients.Select(p => new PatientDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    FullName = p.FullName(),
                    Gender = p.Gender,
                    DateOfBirth = p.DateOfBirth,
                    MedicalRecordNumber = p.MedicalRecordNumber,
                    PhoneNumber = p.PhoneNumber,
                    UserId = p.UserId,
                    Address = p.Address,

                }).ToList()
            };


        }

        private string GeneratePatientMedicalRecordNumber(string firstName, string lastName)
        {
            string first = firstName.Substring(0, 2).ToString().Trim().ToUpper();
            string second = lastName.Substring(1, 2).ToString().Trim().ToUpper();

            return $"{first}{second}{Guid.NewGuid().ToString().Substring(0, 5).Replace("-", "").Trim().ToUpper()}";
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

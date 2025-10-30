using HMS.Identity;
using HMS.Interfaces.Repositories;
using HMS.Interfaces.Services;
using HMS.Models.DTOs;
using HMS.Models.DTOs.Patient;
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
        private readonly IUnitOfWork _unitOfWork;
        ILogger<PatientService> _logger;
        public PatientService(IUserRepository userRepository,
            UserManager<User> userManager,
            IIdentityService identityService,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            ILogger<PatientService> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _identityService = identityService;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<BaseResponse<bool>> CreateAsync(string userToken, CreatePatientRequestModel request)
        {

            var patientExists = await _userRepository.Get<User>(u => u.Email == request.Email);
            if(patientExists != null)
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
                throw new Exception("user Creation Unsuccessful");

               
            }
            var roles = await _roleRepository.GetRolesByIdsAsync(r => request.RoleIds.Contains(r.Id));

            var userRoleNames = roles.Select(r => r.Name).ToList();

            var result = await _userManager.AddToRolesAsync(patientUser, userRoleNames);
            if (!result.Succeeded)
            {
                throw new Exception("Unable to add user to the 'SuperAdmin' role");
            }

            var userRoles = await _userManager.GetRolesAsync(patientUser);

            var token = _identityService.GenerateToken(patientUser, userRoles);

            if (!result.Succeeded)
            {
                throw new Exception($"Unable to add patient to the patient role");
            }

            var patient = new Patient
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                PhoneNumber = request.PhoneNumber,
                
            };

           await _unitOfWork.SaveChangesAsync();

        }

        public Task<bool> DeleteAsync(Guid patientId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<PatientDto>> GetAsync(string param, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<PatientDto>> GetByIdAsync(Guid patientId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<PatientDto>> GetPatientsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private string GeneratePatientMedicalRecordNumber(string firstName, string lastName)
        {
            string firstConcat = firstName.Substring(0, 2).ToString().Trim().ToUpper();
            string secondConcat = lastName.Substring(0, lastName.Length - 1).ToString().Trim().ToUpper();

            return $"{firstConcat}{secondConcat}{Guid.NewGuid().ToString().Substring(0, 5).Replace("-", "").Trim().ToUpper()}";
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
                return (false, "Password must contains one or more characters.");
            }

            // Password is valid
            return (true, null);
        }
    }
}

using HMS.Interfaces.Repositories;
using HMS.Interfaces.Services;
using HMS.Models.DTOs;
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
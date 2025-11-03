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

        public UserService(IUserRepository userRepository,
            UserManager<User> userManager, ILogger<User> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }
        public Task<BaseResponse<UserDto>> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<LoginResponseModel>> Login(LoginRequestModel request, CancellationToken cancellationToken)
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
                        Data = new LoginResponseData
                        {
                            UserId = user.Id,
                            Email = user.Email,
                            Roles = roles.Select(r => new RoleDto { Name = r }).ToList(),
                            Patient = user.Patient != null ? new Models.DTOs.Patients.PatientDto
                            {
                                Id = user.Patient.Id,
                                FullName = user.Patient.FullName(),


                            } : null,


                        }
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
                        Data = new LoginResponseData
                        {
                            UserId = user.Id,
                            Email = user.Email,
                            Roles = roles.Select(r => new RoleDto { Name = r }).ToList(),
                            Doctor = user.Doctor != null ? new Models.DTOs.Doctor.DoctorDto
                            {
                                Id = user.Doctor.Id,
                                FullName = user.Doctor.FullName(),
                            } : null,



                        }
                    }
                };
            }

            return new BaseResponse<LoginResponseModel>
            {
                Message = "Login successful",
                Status = true,
                Data = new LoginResponseModel
                {
                    Data = new LoginResponseData
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        Roles = role.Select(r => new RoleDto { Name = role }).ToList(),


                    }
                }
            };
        }
    }
}
using HMS.Models.DTOs;
using HMS.Models.DTOs.Users;

namespace HMS.Interfaces.Services
{
    public interface IUserService
    {
        public Task<BaseResponse<LoginResponseModel>> LoginAsync(LoginRequestModel request, CancellationToken cancellationToken);
        public Task<BaseResponse<UserDto>> GetUserByEmail(string email, CancellationToken cancellationToken);
        public Task<BaseResponse<UserDto>> GetUserProfileByUserId(Guid userId, CancellationToken cancellationToken);
        public Task<BaseResponse<int>> GetAllHospitalStaffs(CancellationToken cancellationToken);
    }
}

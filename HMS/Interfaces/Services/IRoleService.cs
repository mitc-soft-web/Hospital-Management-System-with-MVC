using HMS.Models.DTOs;
using HMS.Models.DTOs.Role;

namespace HMS.Interfaces.Services
{
    public interface IRoleService
    {
        Task<BaseResponse<RoleDto>> GetByIdAsync(Guid roleId, CancellationToken cancellationToken);
        Task<BaseResponse<IReadOnlyList<RoleDto>>> GetAsync(string param, CancellationToken cancellationToken);
        Task<BaseResponse<IEnumerable<RoleDto>>> GetRolesAsync(CancellationToken cancellationToken);
        //Task<BaseResponse<PatientDto>> UpdateAsync(UpdateProductRequestModel model, string id);
        Task<bool> DeleteAsync(Guid roleId);
    }
}

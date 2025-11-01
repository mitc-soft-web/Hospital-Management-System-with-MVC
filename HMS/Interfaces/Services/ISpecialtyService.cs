using HMS.Models.DTOs;
using HMS.Models.DTOs.Patient;
using HMS.Models.DTOs.Specialty;

namespace HMS.Interfaces.Services
{
    public interface ISpecialtyService
    {
        Task<BaseResponse<bool>> CreateAsync(CreateSpecialtyRequestModel request);
        Task<BaseResponse<SpecialtyDto>> GetByIdAsync(Guid specialtyId, CancellationToken cancellationToken);
        Task<IReadOnlyList<SpecialtyDto>> GetAsync(string param, CancellationToken cancellationToken);
        Task<BaseResponse<IReadOnlyList<SpecialtyDto>>> GetSpecialitiesAsync(CancellationToken cancellationToken);
        //Task<BaseResponse<PatientDto>> UpdateAsync(UpdateProductRequestModel model, string id);
        Task<BaseResponse<bool>> DeleteAsync(Guid specialtyId);
    }
}

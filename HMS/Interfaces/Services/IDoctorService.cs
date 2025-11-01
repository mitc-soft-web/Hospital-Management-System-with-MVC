using HMS.Models.DTOs;
using HMS.Models.DTOs.Doctor;

namespace HMS.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<BaseResponse<bool>> CreateAsync(CreateDoctorRequestModel request);
        Task<BaseResponse<DoctorDto>> GetByIdAsync(Guid dotorId, CancellationToken cancellationToken);
        Task<IReadOnlyList<DoctorDto>> GetAsync(string param, CancellationToken cancellationToken);
        Task<BaseResponse<IReadOnlyList<DoctorDto>>> GetDotorsAsync(CancellationToken cancellationToken);
        //Task<BaseResponse<PatientDto>> UpdateAsync(UpdateProductRequestModel model, string id);
        Task<BaseResponse<bool>> DeleteAsync(Guid doctorId);
    }
}

using HMS.Models.DTOs;
using HMS.Models.DTOs.Patient;

namespace HMS.Interfaces.Services
{
    public interface IPatientService
    {
        Task<BaseResponse<bool>> CreateAsync(string userToken, CreatePatientRequestModel request);
        Task<BaseResponse<PatientDto>> GetByIdAsync(Guid patientId, CancellationToken cancellationToken);
        Task<IReadOnlyList<PatientDto>> GetAsync(string param, CancellationToken cancellationToken);
        Task<IReadOnlyList<PatientDto>> GetPatientsAsync(CancellationToken cancellationToken);
        //Task<BaseResponse<PatientDto>> UpdateAsync(UpdateProductRequestModel model, string id);
        Task<bool> DeleteAsync(Guid patientId);
    }
}

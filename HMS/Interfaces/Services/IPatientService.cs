using HMS.Models.DTOs;
using HMS.Models.DTOs.Patient;
using HMS.Models.DTOs.Patients;

namespace HMS.Interfaces.Services
{
    public interface IPatientService
    {
        Task<BaseResponse<bool>> CreateAsync(CreatePatientRequestModel request);
        Task<BaseResponse<PatientDto>> GetByIdAsync(Guid patientId, CancellationToken cancellationToken);
        Task<IReadOnlyList<PatientDto>> GetAsync(string param, CancellationToken cancellationToken);
        Task<BaseResponse<IReadOnlyList<PatientDto>>> GetPatientsAsync(CancellationToken cancellationToken);
        //Task<BaseResponse<PatientDto>> UpdateAsync(UpdateProductRequestModel model, string id);
        Task<BaseResponse<bool>> DeleteAsync(Guid patientId);
    }
}

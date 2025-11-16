using HMS.Models.DTOs;
using HMS.Models.DTOs.Appointment;
using HMS.Models.DTOs.Patient;
using HMS.Models.DTOs.Patients;

namespace HMS.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<BaseResponse<bool>> CreateAsync(CreateAppointmentRequestModel request);
        Task<BaseResponse<AppointmentDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<BaseResponse<IReadOnlyList<AppointmentDto>>> GetAllByDoctorIdAsync(Guid userDoctorId, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> AcceptAppointment(Guid id);
        Task<BaseResponse<IReadOnlyList<AppointmentDto>>> GetAppointmentsAsync(CancellationToken cancellationToken);
        Task<BaseResponse<IReadOnlyList<AppointmentDto>>> GetScheduledAppointmentsAsync(CancellationToken cancellationToken);
        Task<BaseResponse<IReadOnlyList<AppointmentDto>>> GetCancelledAppointmentsAsync(CancellationToken cancellationToken);
        Task<BaseResponse<IReadOnlyList<AppointmentDto>>> GetRescheduledAppointmentsAsync(CancellationToken cancellationToken);
        Task<BaseResponse<IReadOnlyList<AppointmentDto>>> GetCompletedAppointmentsAsync(CancellationToken cancellationToken);
        //Task<BaseResponse<PatientDto>> UpdateAsync(UpdateProductRequestModel model, string id);
        Task<BaseResponse<bool>> DeleteAsync(Guid patientId);
    }
}

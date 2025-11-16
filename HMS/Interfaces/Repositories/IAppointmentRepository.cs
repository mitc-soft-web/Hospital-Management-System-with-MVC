using HMS.Models.Entities;

namespace HMS.Interfaces.Repositories
{
    public interface IAppointmentRepository : IBaseRepository
    {
        Task<IReadOnlyList<Appointment>> GetAllScheduledAppointments();
        Task<IReadOnlyList<Appointment>> GetAllRescheduledAppointments();
        Task<IReadOnlyList<Appointment>> GetAllCancelledAppointments();
        Task<IReadOnlyList<Appointment>> GetAllCompletedAppointments();
        Task<IReadOnlyList<Appointment>> GetAllAppointmentsAndDetails();

        bool AcceptAppointment(Appointment appointment);
        Task<Appointment> GetAppointmentById(Guid id);
        Task<IReadOnlyList<Appointment>> GetAppointmentByDctorId(Guid userDoctorId);

    }
}

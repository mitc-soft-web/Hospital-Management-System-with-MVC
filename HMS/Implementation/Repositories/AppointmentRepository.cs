using HMS.Interfaces.Repositories;
using HMS.Models.Entities;
using HMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HMS.Implementation.Repositories
{
    public class AppointmentRepository : BaseRespository, IAppointmentRepository
    {
        public AppointmentRepository(HmsContext hmsContext) : base(hmsContext)
        {
        }

        public async Task<IReadOnlyList<Appointment>> GetAllCancelledAppointments()
        {
            return await _hmsContext.Set<Appointment>()
                .Where(ap => ap.AppointmentStatus == Models.Enums.AppointmentStatus.Cancelled)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Appointment>> GetAllCompletedAppointments()
        {
            return await _hmsContext.Set<Appointment>()
               .Where(ap => ap.AppointmentStatus == Models.Enums.AppointmentStatus.Completed)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IReadOnlyList<Appointment>> GetAllRescheduledAppointments()
        {
            return await _hmsContext.Set<Appointment>()
               .Where(ap => ap.AppointmentStatus == Models.Enums.AppointmentStatus.Rescheduled)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IReadOnlyList<Appointment>> GetAllScheduledAppointments()
        {
            return await _hmsContext.Set<Appointment>()
               .Where(ap => ap.AppointmentStatus == Models.Enums.AppointmentStatus.Scheduled)
               .AsNoTracking()
               .ToListAsync();
        }
    }
}

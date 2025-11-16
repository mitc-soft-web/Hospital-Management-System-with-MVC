using HMS.Interfaces.Repositories;
using HMS.Models.Entities;
using HMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NuGet.Versioning;
using System.Threading;

namespace HMS.Implementation.Repositories
{
    public class AppointmentRepository : BaseRespository, IAppointmentRepository
    {
        public AppointmentRepository(HmsContext hmsContext) : base(hmsContext)
        {
        }

        public bool AcceptAppointment(Appointment appointment)
        {
            _hmsContext.Set<Appointment>()
               .Update(appointment);
            return true;
                
        }

        public async Task<IReadOnlyList<Appointment>> GetAllAppointmentsAndDetails()
        {
            return await _hmsContext.Set<Appointment>()
                .OrderByDescending(d => d.DateCreated)
                .Include(ap => ap.Patient)
                .ThenInclude(p => p.User)
                .Include(ap => ap.Doctor)
                .ThenInclude(d => d.User)
                .Include(d => d.Doctor)
                .ThenInclude(ap => ap.DoctorSpecialities)
                .ThenInclude(ap => ap.Speciality)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Appointment>> GetAllCancelledAppointments()
        {
            return await _hmsContext.Set<Appointment>()
                .OrderByDescending (d => d.DateCreated)
                .Include(ap => ap.Doctor)
                .Include(ap => ap.Patient)
                .Where(ap => ap.AppointmentStatus == Models.Enums.AppointmentStatus.Cancelled)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Appointment>> GetAllCompletedAppointments()
        {
            return await _hmsContext.Set<Appointment>()
                .OrderByDescending(d => d.DateCreated)
                .Include(ap => ap.Doctor)
                .Include(ap => ap.Patient)
               .Where(ap => ap.AppointmentStatus == Models.Enums.AppointmentStatus.Completed)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IReadOnlyList<Appointment>> GetAllRescheduledAppointments()
        {
            return await _hmsContext.Set<Appointment>()
                .OrderByDescending(d => d.DateCreated)
                .Include(ap => ap.Doctor)
                .Include(ap => ap.Patient)
               .Where(ap => ap.AppointmentStatus == Models.Enums.AppointmentStatus.Rescheduled)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IReadOnlyList<Appointment>> GetAllScheduledAppointments()
        {
            return await _hmsContext.Set<Appointment>()
                .OrderByDescending(d => d.DateCreated)
                .Include(ap => ap.Doctor)
                .Include(ap => ap.Patient)
               .Where(ap => ap.AppointmentStatus == Models.Enums.AppointmentStatus.Scheduled)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IReadOnlyList<Appointment>> GetAppointmentByDctorId(Guid userDoctorId)
        {
            return await _hmsContext.Set<Appointment>()
                .Where(ap => ap.Doctor.UserId == userDoctorId)
                .OrderByDescending(d => d.DateCreated)
                .Include(ap => ap.Patient)
                .ThenInclude(p => p.User)
                .Include(ap => ap.Patient)
                .ThenInclude(p => p.PatientDetail)
                .Include(ap => ap.Doctor)
                .ThenInclude(d => d.User)
                .Include(ap => ap.Doctor)
                .ThenInclude(d => d.DoctorSpecialities)
                .ThenInclude(ds => ds.Speciality)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<Appointment> GetAppointmentById(Guid id)
        {
                return await _hmsContext.Set<Appointment>()
            .Where(a => a.Id == id)
            .Include(a => a.Patient)
                .ThenInclude(p => p.User)
            .Include(a => a.Patient)
                .ThenInclude(p => p.PatientDetail)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.DoctorSpecialities)
                    .ThenInclude(ds => ds.Speciality)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync();

        }
    }
}

using HMS.Interfaces.Repositories;
using HMS.Models.Entities;
using HMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HMS.Implementation.Repositories
{
    public class DoctorRepository : BaseRespository, IDoctorRepository
    {

        public DoctorRepository(HmsContext hmsContext) : base(hmsContext)
        {

        }

        public async Task<IReadOnlyList<Doctor>> GetAllDoctorsAndTheirSpecialities()
        {
            return await _hmsContext.Set<Doctor>()
                .Include(d => d.User)
                .Include(d => d.DoctorSpecialities)
                .ThenInclude(ds => ds.Speciality)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<List<Doctor>> GetDoctorsBySpeciality(string specialityName)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetDoctorCounts()
        {
            return await _hmsContext.Set<Doctor>()
                .CountAsync();
        }

        public async Task<Doctor> GetDoctorByIdAsync(Guid doctorId)
        {
            return await _hmsContext.Set<Doctor>()
                .Include(d => d.User)
                .Include(d => d.DoctorSpecialities)
                .ThenInclude(ds => ds.Speciality)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == doctorId);
        }
    }
}

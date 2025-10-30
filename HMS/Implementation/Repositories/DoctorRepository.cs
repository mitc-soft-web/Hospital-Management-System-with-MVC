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

        public async Task<List<Doctor>> GetDoctorsBySpeciality(string specialityName)
        {

            return await _hmsContext.Set<Doctor>()
                .Where(d => d.DoctorSpecialities
                .Any(ds => ds.Speciality.Name == specialityName))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Doctor>> GetAllDoctorsAndTheirSpecialities()
        {
            return await _hmsContext.Set<Doctor>()
                .Include(d => d.DoctorSpecialities)
                .ThenInclude(d => d.Speciality)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}

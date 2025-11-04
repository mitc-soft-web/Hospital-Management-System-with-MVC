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

        public Task<IReadOnlyList<Doctor>> GetAllDoctorsAndTheirSpecialities()
        {
            throw new NotImplementedException();
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
    }
}

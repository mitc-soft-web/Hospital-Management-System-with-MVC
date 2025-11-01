using HMS.Interfaces.Repositories;
using HMS.Models.Entities;
using HMS.Persistence.Context;

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
    }
}

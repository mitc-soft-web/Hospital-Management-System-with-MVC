using HMS.Interfaces;
using HMS.Models.Entities;
using HMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HMS.Implementation
{
    public class DoctorRepository : BaseRespository, IDoctorRepository
    {
       
        public DoctorRepository(HmsContext hmsContext) : base(hmsContext)
        {
           
        }

        public async Task<List<Doctor>> GetDoctorsBySpeciality(string specialityName)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Doctor>> GetAllDoctorsAndTheirSpecialities()
        {
            throw new NotImplementedException();
        }

    }
}

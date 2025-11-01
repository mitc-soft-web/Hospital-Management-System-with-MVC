using HMS.Interfaces.Repositories;
using HMS.Models.Entities;
using HMS.Persistence.Context;

namespace HMS.Implementation.Repositories
{
    public class PatientRepository : BaseRespository, IPatientRepository
    {
        public PatientRepository(HmsContext hmsContext) : base(hmsContext)
        {
           
        }

        public async Task<Patient> Add(Patient entity)
        {
           var patient = await _hmsContext.AddAsync(entity);
            return entity;

        }
    }
}

using HMS.Interfaces.Repositories;
using HMS.Persistence.Context;

namespace HMS.Implementation.Repositories
{
    public class PatientRepository : BaseRespository, IPatientRepository
    {
        public PatientRepository(HmsContext hmsContext) : base(hmsContext)
        {
           
        }
    }
}

using HMS.Interfaces;
using HMS.Persistence.Context;

namespace HMS.Implementation
{
    public class PatientRepository : BaseRespository, IPatientRepository
    {
        public PatientRepository(HmsContext hmsContext) : base(hmsContext)
        {
           
        }
    }
}

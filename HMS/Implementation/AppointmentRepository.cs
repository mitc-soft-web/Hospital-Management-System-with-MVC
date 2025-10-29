using HMS.Interfaces;
using HMS.Persistence.Context;

namespace HMS.Implementation
{
    public class AppointmentRepository : BaseRespository, IAppointmentRepository
    {
        public AppointmentRepository(HmsContext hmsContext) : base(hmsContext)
        {
        }
    }
}

using HMS.Interfaces.Repositories;
using HMS.Persistence.Context;

namespace HMS.Implementation.Repositories
{
    public class RoleRepository : BaseRespository, IRoleRepository
    {
        public RoleRepository(HmsContext hmsContext) : base(hmsContext)
        {

        }
    }
}

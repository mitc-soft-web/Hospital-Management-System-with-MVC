using HMS.Interfaces.Repositories;
using HMS.Models.Entities;
using HMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HMS.Implementation.Repositories
{
    public class RoleRepository : BaseRespository, IRoleRepository
    {
        public RoleRepository(HmsContext hmsContext) : base(hmsContext)
        {

        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            return await _hmsContext.Set<Role>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Role>> GetRolesByIdsAsync(Expression<Func<Role, bool>> expression)
        {
            return await _hmsContext.Set<Role>()
                .Where(expression)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}

using HMS.Models.Contracts;
using HMS.Models.Entities;
using System.Linq.Expressions;

namespace HMS.Interfaces.Repositories
{
    public interface IRoleRepository : IBaseRepository
    {

        Task<IEnumerable<Role>> GetRolesByIdsAsync(Expression<Func<Role, bool>> expression);
    }
}

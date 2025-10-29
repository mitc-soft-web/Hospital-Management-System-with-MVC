using HMS.Interfaces;
using HMS.Models.Entities;
using HMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HMS.Implementation
{
    public class UserRespository : BaseRespository, IUserRepository
    {
        public UserRespository(HmsContext hmsContext) : base(hmsContext)
        {
        }

        public async Task<IReadOnlyList<User>> GetByRole(Expression<Func<User, bool>> expression)
        {
            return await _hmsContext.Set<User>()
                .Where(expression)
                .AsNoTracking()
                .ToListAsync();
         
        }
    }

}

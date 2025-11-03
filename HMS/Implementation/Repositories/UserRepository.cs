using HMS.Interfaces.Repositories;
using HMS.Models.Entities;
using HMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HMS.Implementation.Repositories
{
    public class UserRepository : BaseRespository, IUserRepository
    {
        public UserRepository(HmsContext hmsContext) : base(hmsContext)
        {
        }

        public async Task<bool> Any(Expression<Func<User, bool>> expression)
        {
            return await _hmsContext.Set<User>()
                .AnyAsync(expression);

        }

        public async Task<IReadOnlyList<User>> GetByRole(Expression<Func<User, bool>> expression)
        {
            return await _hmsContext.Set<User>()
                .Where(expression)
                .AsNoTracking()
                .ToListAsync();

        }

        public async Task<User> GetUserAndRoles(Guid userId)
        {
            return await _hmsContext.Set<User>()
               .Where(u => u.Id == userId)
               .Include(u => u.UserRoles)
               .ThenInclude(u => u.Role)
               .SingleOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _hmsContext.Set<User>()
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .Include(d => d.Doctor)
                .Include(p => p.Patient)
                .ThenInclude(p => p.PatientDetail)
                .SingleOrDefaultAsync(u => u.Email == email);
        }

       
    }

}

using HMS.Models.Entities;
using System.Linq.Expressions;

namespace HMS.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository
    {
        public Task<IReadOnlyList<User>> GetByRole(Expression<Func<User, bool>> expression);
        public Task<User> GetUserAndRoles(Guid userId);
        Task<User> GetUserByEmail(string email);
        Task<bool> Any(Expression<Func<User, bool>> expression);
    }
}

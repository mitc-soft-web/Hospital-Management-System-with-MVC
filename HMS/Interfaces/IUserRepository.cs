using HMS.Models.Entities;
using System.Linq.Expressions;

namespace HMS.Interfaces
{
    public interface IUserRepository : IBaseRepository
    {
       public Task<IReadOnlyList<User>> GetByRole(Expression<Func<User, bool>> expression);
    }
}

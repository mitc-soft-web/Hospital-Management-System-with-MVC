using HMS.Models.Entities;
using System.Linq.Expressions;

namespace HMS.Interfaces.Repositories
{
    public interface ISpecialityRepository : IBaseRepository
    {
        Task<bool> Any(Expression<Func<Speciality, bool>> expression);
    }
}

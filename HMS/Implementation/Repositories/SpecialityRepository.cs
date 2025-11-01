using HMS.Interfaces.Repositories;
using HMS.Models.Entities;
using HMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HMS.Implementation.Repositories
{
    public class SpecialityRepository : BaseRespository, ISpecialityRepository
    {
        public SpecialityRepository(HmsContext hmsContext) : base(hmsContext)
        {

        }

        public async Task<bool> Any(Expression<Func<Speciality, bool>> expression)
        {
            return await _hmsContext.Set<Speciality>()
                .AnyAsync(expression);

        }
    }
}

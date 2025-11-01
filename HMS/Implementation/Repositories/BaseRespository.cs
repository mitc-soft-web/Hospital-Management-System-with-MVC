using HMS.Contracts.Entities;
using HMS.Interfaces.Repositories;
using HMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HMS.Implementation.Repositories
{
    public class BaseRespository : IBaseRepository
    {
        protected readonly HmsContext _hmsContext;
        public BaseRespository(HmsContext hmsContext)
        {
            _hmsContext = hmsContext ?? throw new ArgumentNullException(nameof(hmsContext));
        }
        public virtual void Add<T>(T entity) where T : BaseEntity => _hmsContext.Add(entity);
        
        public virtual void Delete<T>(T entity) where T : BaseEntity
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> Get<T>(Expression<Func<T, bool>> expression) where T : BaseEntity
        {
            return await _hmsContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public virtual async Task<IReadOnlyList<T>> GetAll<T>() where T : BaseEntity
        {
            return await _hmsContext.Set<T>()
                .ToListAsync();
        }


        public IQueryable<T> QueryWhere<T>(Expression<Func<T, bool>> expression) where T : BaseEntity
        {
            return _hmsContext.Set<T>().Where(expression);
        }

        public void Update<T>(T entity) where T : BaseEntity => _hmsContext.Set<T>().Update(entity);

    }
}

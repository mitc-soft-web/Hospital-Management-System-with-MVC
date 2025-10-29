using HMS.Interfaces;
using HMS.Persistence.Context;

namespace HMS.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HmsContext _hmsContext;

        public UnitOfWork(HmsContext hmsContext)
        {
           _hmsContext = hmsContext ?? throw new ArgumentNullException(nameof(hmsContext));
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
           return await _hmsContext.SaveChangesAsync(cancellationToken);
        }
    }
}

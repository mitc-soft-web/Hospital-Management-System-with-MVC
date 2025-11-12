using HMS.Interfaces.Repositories;
using HMS.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace HMS.Implementation.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HmsContext _hmsContext;

        public UnitOfWork(HmsContext hmsContext)
        {
            _hmsContext = hmsContext ?? throw new ArgumentNullException(nameof(hmsContext));
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _hmsContext.Database.BeginTransactionAsync();
        }

        public IExecutionStrategy CreateExecutionStrategy()
        {
            return _hmsContext.Database.CreateExecutionStrategy();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _hmsContext.SaveChangesAsync(cancellationToken);
        }
    }
}

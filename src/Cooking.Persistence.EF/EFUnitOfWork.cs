using System.Threading.Tasks;
using Cooking.Infrastructure.Application;

namespace Cooking.Persistence.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly EFDataContext _dataContext;

        public EFUnitOfWork(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task BeginAsync()
        {
            await _dataContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _dataContext.SaveChangesAsync();
            _dataContext.Database.CommitTransaction();
        }

        public async Task CommitPartialAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task CompleteAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public void Rollback()
        {
            _dataContext.Database.RollbackTransaction();
        }
    }
}
using System.Threading.Tasks;
using Cooking.Infrastructure.Application;

namespace Cooking.Persistence.EF
{
    public class EFUnitOfWork : UnitOfWork
    {
        private readonly EFDataContext _dataContext;

        public EFUnitOfWork(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Begin()
        {
            await _dataContext.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _dataContext.SaveChangesAsync();
            _dataContext.Database.CommitTransaction();
        }

        public async Task CommitPartial()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task Complete()
        {
            await _dataContext.SaveChangesAsync();
        }

        public void Rollback()
        {
            _dataContext.Database.RollbackTransaction();
        }
    }
}
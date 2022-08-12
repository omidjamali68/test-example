using System.Threading.Tasks;

namespace TestExample.Infrastructure.Application
{
    public interface IUnitOfWork
    {
        Task BeginAsync();
        void CommitPartial();
        Task CommitAsync();
        void Rollback();
        Task CompleteAsync();
    }
}
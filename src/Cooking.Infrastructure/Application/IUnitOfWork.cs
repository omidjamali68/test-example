using System.Threading.Tasks;

namespace Cooking.Infrastructure.Application
{
    public interface IUnitOfWork
    {
        Task BeginAsync();
        Task CommitPartialAsync();
        Task CommitAsync();
        void Rollback();
        Task CompleteAsync();
    }
}
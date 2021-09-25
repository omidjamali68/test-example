using System.Threading.Tasks;

namespace Cooking.Infrastructure.Application
{
    public interface UnitOfWork
    {
        Task Begin();
        Task CommitPartial();
        Task Commit();
        void Rollback();
        Task Complete();
    }
}
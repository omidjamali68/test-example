using TestExample.Entities.Masters;
using TestExample.Infrastructure.Application;

namespace TestExample.Services.Masters.Contracts
{
    public interface IMasterRepository : IRepository
    {
        void Add(Master master);
    }
}
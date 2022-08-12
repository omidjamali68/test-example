using System.Threading.Tasks;
using TestExample.Infrastructure.Application;

namespace TestExample.Services.Masters.Contracts
{
    public interface IMasterService : IService
    {
        Task<int> Add(AddMasterDto dto);
    }
}
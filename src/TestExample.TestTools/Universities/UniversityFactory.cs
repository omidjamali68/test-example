using TestExample.Persistence.EF;
using TestExample.Persistence.EF.Universities;
using TestExample.Services.Universities;

namespace TestExample.TestTools.Universities
{
    public static class UniversityFactory
    {
        public static UniversityAppService CreateService(EFDataContext context)
        {
            var repository = new EFUniversityRepository(context);
            var unitOfWork = new EFUnitOfWork(context);
            return new UniversityAppService(repository, unitOfWork);
        }
    }
}
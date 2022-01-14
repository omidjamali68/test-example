using Cooking.Persistence.EF;
using Cooking.Persistence.EF.StatePersistence.Nationalities;
using Cooking.Services.StateServices.Nationalities;

namespace Cooking.TestTools.StateTestTools
{
    public class NationalityFactory
    {
        public static NationaliryAppService CreateService(EFDataContext context)
        {
            var repository = new EFNationalityRepository(context);
            return new NationaliryAppService(repository);
        }
    }
}
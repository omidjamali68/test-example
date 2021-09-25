using Cooking.Entities.States;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;

namespace Cooking.TestTools.StateTestTools
{
    public class CityBuilder
    {
        private readonly City _city = new City();

        public CityBuilder(int provinceId)
        {
            _city.ProvinceId = provinceId;
        }

        public CityBuilder WithTitle(string title)
        {
            _city.Title = title;
            return this;
        }

        public City Build(EFDataContext context)
        {
            context.Manipulate(_ => _.Set<City>().Add(_city));
            return _city;
        }
    }
}
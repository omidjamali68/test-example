using Cooking.Entities.States;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;

namespace Cooking.TestTools.StateTestTools
{
    public class ProvinceBuilder
    {
        private readonly Province _province = new Province();

        public ProvinceBuilder WithTitle(string title)
        {
            _province.Title = title;
            return this;
        }

        public Province Build(EFDataContext context)
        {
            context.Manipulate(_ => _.Set<Province>().Add(_province));
            return _province;
        }
    }
}
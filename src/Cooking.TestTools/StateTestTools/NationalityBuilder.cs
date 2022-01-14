using Cooking.Entities.States;
using Cooking.Infrastructure.Test;
using Cooking.Persistence.EF;

namespace Cooking.TestTools.StateTestTools
{
    public class NationalityBuilder
    {
        private Nationality _nationality = new Nationality { 
            Name = "dummy_name"
        };

        public NationalityBuilder WithName(string name)
        {
            _nationality.Name = name;
            return this;
        }
        public Nationality Build(EFDataContext context)
        {
            context.Manipulate(_ => _.Nationalities.Add(_nationality));
            return _nationality;
        }
    }
}

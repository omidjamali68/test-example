using Cooking.Specs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooking.Specs.Recipes.StepOperations.Add
{
    [Scenario("تعریف مرحله پخت غذا به همراه تصویر آیکون")]
    public class Successful : EFDataContextDatabaseFixture
    {
        public Successful(ConfigurationFixture configuration) : base(configuration)
        {
        }
    }
}

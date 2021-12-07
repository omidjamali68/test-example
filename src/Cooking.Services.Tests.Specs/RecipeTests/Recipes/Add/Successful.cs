using Cooking.Entities.Documents;
using Cooking.Persistence.EF;
using Cooking.Services.RecipeServices.Recipes.Contracts;
using Cooking.Specs.Infrastructure;
using Cooking.TestTools.DocumentTestTools;
using Cooking.TestTools.IngredientTestTools.IngredientUnits;
using Cooking.TestTools.RecipeTestTools.Recipes;
using Cooking.TestTools.RecipeTestTools.StepOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cooking.Specs.Recipes.Recipes.Add
{
    [Scenario("تعریف دستور پخت غذا به همراه فایل تصاویر")]
    public class Successful : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _readDataContext;
        private readonly EFDataContext _context;
        private readonly IRecipeService _sut;
        public Successful(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _readDataContext = CreateDataContext();
            _sut = RecipeFactory.CreateService(_context);
        }

        [Given("یک مرحله پخت غذا با عنوان: سرخ کردن در فهرست مراحل پخت غذا وجود دارد")]
        [And("یک واحد مواد اولیه با عنوان: تعداد، در فهرست واحدهای مواد اولیه وجود دارد")]
        private void Given()
        {
            var doc = DocumentFactory.CreateDocument(_context, DocumentStatus.Reserve);
            var operationStep = new StepOperationBuilder(doc)
                .Build(_context);
            var ingredientUnit = new IngredientUnitBuilder()
                .WithTitle("تعداد")
                .Build(_context);
        }

        [And("یک واحد مواد اولیه با عنوان: گرم، در فهرست واحدهای مواد اولیه وجود دارد")]
        private void And_IngredientUnit_Exist()
        {

        }

        [And("یک ماده اولیه با عنوان تخم مرغ و واحد تعداد در فهرست مواد اولیه وجود دارد")]
        private void And_First_Ingredient_Exist()
        {

        }

        [And("یک ماده اولیه با عنوان روغن و واحد گرم در فهرست مواد اولیه وجود دارد")]
        private void And_Second_Ingredient_Exist()
        {

        }

        [When("یک دستور پخت با نام غذا: نیمرو، زمان آماده سازی: 5 دقیقه و گروه دستور پخت: حاضری و کشور: ایران و مرحله" +
        " پخت با عنوان: سرخ کردن به همراه فایل های مربوطه در فهرست دستور پخت ها تعریف میکنم")]
        private async Task When()
        {

        }

        [Then("باید یک دستور پخت با نام غذا: نیمرو،" +
        " زمان آماده سازی: 5 دقیقه و گروه دستور پخت: حاضری و کشور: ایران و مرحله پخت" +
        " با عنوان: سرخ کردن به همراه فایل های مربوطه در فهرست دستور پخت ها وجود داشته باشد")]
        private void Then()
        {

        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given(),
                _ => And_IngredientUnit_Exist(),
                _ => And_First_Ingredient_Exist(),
                _ => And_Second_Ingredient_Exist(),
                _ => When().Wait(),
                _ => Then()
                );
        }
    }
}

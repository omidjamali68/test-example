using FluentMigrator;

namespace Cooking.Migrations
{
    [Migration(202105061313)]
    public class _202105061313_Initial_Iran_Data_Inserted : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("IranGeoData.sql");
            Execute.EmbeddedScript("InitialIngredientUnits.sql");
            Execute.EmbeddedScript("InitialRecipeCategories.sql");
            Execute.EmbeddedScript("InitialNationalities.sql");

        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM Nationalities;");
            Execute.Sql("DELETE FROM RecipeCategories;");
            Execute.Sql("DELETE FROM IngredientUnits;");
            Execute.Sql("DELETE FROM Cities;");
            Execute.Sql("DELETE FROM Provinces;");
        }
    }
}
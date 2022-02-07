using FluentMigrator;

namespace Cooking.Migrations
{
    [Migration(202202022251)]
    public class _202202022251_AddRecipeMainDocument : Migration
    {
        public override void Down()
        {
            Delete.Column("MainDocumentExtension").FromTable("Recipes");
            Delete.Column("MainDocumentId").FromTable("Recipes");
        }

        public override void Up()
        {
            Alter.Table("Recipes").AddColumn("MainDocumentId").AsGuid().NotNullable().WithDefaultValue("00000000-0000-0000-0000-000000000000");
            Alter.Table("Recipes").AddColumn("MainDocumentExtension").AsString(10).NotNullable().WithDefaultValue("");
        }
    }
}
using System;
using System.Data;
using Cooking.Migrations.Scripts;
using FluentMigrator;

namespace Cooking.Migrations
{
    [Migration(202105051419)]
    public class _202105051419_InitialDatabase : Migration
    {
        private readonly ScriptResourceManager _sourceManager;

        public _202105051419_InitialDatabase(ScriptResourceManager sourceManager)
        {
            _sourceManager = sourceManager;
        }

        public override void Up()
        {
            #region States

            Create.Table("Provinces")
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("Title").AsString(50).NotNullable();

            Create.Table("Cities")
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("Title").AsString(50).NotNullable()
                .WithColumn("ProvinceId").AsInt32().NotNullable()
                .ForeignKey("FK_Provinces_Cities", "Provinces", "Id").OnDelete(Rule.None);

            Create.Table("Nationalities")
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("Name").AsString(100).NotNullable();

            #endregion

            #region ApplicationIdentities

            var script = _sourceManager.Read("UserManagement_Initial.sql");
            Execute.Sql(script);

            #endregion

            #region Documents

            Create.Table("Documents")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Data").AsBinary(Int32.MaxValue)
                .WithColumn("FileName").AsString(50).Nullable()
                .WithColumn("Extension").AsString(10).Nullable()
                .WithColumn("Status").AsInt16().NotNullable()
                .WithColumn("CreationDate").AsDateTime2().NotNullable();

            #endregion

            #region Ingredients

            Create.Table("IngredientUnits")
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("Title").AsString(50).NotNullable();

            Create.Table("Ingredients")
                .WithColumn("Id").AsInt64().PrimaryKey().NotNullable().Identity()
                .WithColumn("Title").AsString(70).NotNullable()
                .WithColumn("AvatarId").AsGuid().NotNullable()
                .WithColumn("Extension").AsString(10).NotNullable()
                .WithColumn("IngredientUnitId").AsInt32().NotNullable()
                .ForeignKey("FK_IngredientUnits_Ingredients", "IngredientUnits", "Id")
                .OnDelete(Rule.None);

            #endregion

            #region Recipes

            Create.Table("RecipeCategories")
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("Title").AsString(50).NotNullable();

            Create.Table("Recipes")
                .WithColumn("Id").AsInt64().PrimaryKey().NotNullable().Identity()
                .WithColumn("FoodName").AsString(100).NotNullable()
                .WithColumn("Duration").AsInt16().Nullable()
                .WithColumn("RecipeCategoryId").AsInt32().NotNullable()
                .ForeignKey("FK_RecipeCategories_Recipes", "RecipeCategories", "Id")
                .OnDelete(Rule.None)
                .WithColumn("NationalityId").AsInt32().NotNullable()
                .ForeignKey("FK_Nationalities_Recipes", "Nationalities", "Id")
                .OnDelete(Rule.None);

            Create.Table("RecipeDocuments")
                .WithColumn("RecipeId").AsInt64().NotNullable()
                .ForeignKey("FK_Recipes_RecipeDocuments", "Recipes", "Id")
                .OnDelete(Rule.Cascade)
                .WithColumn("DocumentId").AsGuid().NotNullable()
                .WithColumn("Extension").AsString(10).NotNullable();

            Create.Table("RecipeIngredients")
                .WithColumn("Quantity").AsDouble().NotNullable()
                .WithColumn("RecipeId").AsInt64().PrimaryKey().NotNullable()
                .ForeignKey("FK_Recipes_RecipeIngredients", "Recipes", "Id")
                .OnDelete(Rule.Cascade)
                .WithColumn("IngredientId").AsInt64().PrimaryKey().NotNullable()
                .ForeignKey("FK_Ingredients_RecipeIngredients", "Ingredients", "Id")
                .OnDelete(Rule.None);

            Create.Table("StepOperations")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("Title").AsString(100).NotNullable()
                .WithColumn("AvatarId").AsGuid().NotNullable();

            Create.Table("StepOperationDocuments")
                .WithColumn("StepOperationId").AsInt64().NotNullable()
                .ForeignKey("FK_StepOperations_StepOperationDocuments", "StepOperations", "Id")
                .OnDelete(Rule.Cascade)
                .WithColumn("DocumentId").AsGuid().NotNullable()
                .WithColumn("Extension").AsString(10).NotNullable();

            Create.Table("RecipeSteps")
                .WithColumn("Order").AsInt16().NotNullable()
                .WithColumn("Description").AsString(1000).NotNullable()
                .WithColumn("RecipeId").AsInt64().NotNullable()
                .ForeignKey("FK_Recipes_RecipeSteps", "Recipes", "Id")
                .OnDelete(Rule.Cascade)
                .WithColumn("StepOperationId").AsInt64().NotNullable()
                .ForeignKey("FK_StepOperations_RecipeStep", "StepOperations", "Id")
                .OnDelete(Rule.None);

            #endregion
        }

        public override void Down()
        {
            #region Recipes

            Delete.Table("RecipeSteps");
            Delete.Table("StepOperationDocuments");
            Delete.Table("StepOperations");
            Delete.Table("RecipeIngredient");
            Delete.Table("RecipeDocuments");
            Delete.Table("Recipes");
            Delete.Table("RecipeCategories");

            #endregion

            #region Ingredients

            Delete.Table("Ingredients");
            Delete.Table("IngredientUnits");

            #endregion

            #region ApplicationIdentities

            Delete.Table("ApplicationUserTokens");
            Delete.Table("ApplicationUserRoles");
            Delete.Table("ApplicationUserLogins");
            Delete.Table("ApplicationUserClaims");
            Delete.Table("ApplicationRoleClaims");
            Delete.Table("ApplicationRoles");
            Delete.Table("ApplicationUsers");

            #endregion

            #region States

            Delete.Table("Nationalities");
            Delete.Table("Cities");
            Delete.Table("Provinces");

            #endregion
        }
    }
}
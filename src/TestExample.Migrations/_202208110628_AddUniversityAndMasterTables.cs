using FluentMigrator;

namespace TestExample.Migrations
{
    [Migration(202208110628)]
    public class _202208110628_AddUniversityAndMasterTables : Migration
    {
        public override void Up()
        {
            Create.Table("Universities")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("Email").AsString(200).NotNullable()
                .WithColumn("Address").AsString(200).NotNullable();

            Create.Table("Masters")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("FirstName").AsString(100).NotNullable()
                .WithColumn("LastName").AsString(100).NotNullable()
                .WithColumn("NationalCode").AsString(10).NotNullable()
                .WithColumn("Mobile").AsString(11).NotNullable()
                .WithColumn("UniversityId").AsInt32().NotNullable()
                .ForeignKey("FK_Universities_Masters", "Universities", "Id")
                .WithColumn("UserId").AsGuid().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Masters");
            Delete.Table("Universities");
        }
    }
}
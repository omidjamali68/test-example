using FluentMigrator;
using TestExample.Migrations.Scripts;

namespace TestExample.Migrations
{
    [Migration(202208102200)]
    public class _202208102200_InitialDatabase : Migration
    {
        private readonly ScriptResourceManager _sourceManager;

        public _202208102200_InitialDatabase(ScriptResourceManager sourceManager)
        {
            _sourceManager = sourceManager;
        }

        public override void Up()
        {
            var script = _sourceManager.Read("UserManagement_Initial.sql");
            Execute.Sql(script);
        }

        public override void Down()
        {
            Delete.Table("ApplicationUserTokens");
            Delete.Table("ApplicationUserRoles");
            Delete.Table("ApplicationUserLogins");
            Delete.Table("ApplicationUserClaims");
            Delete.Table("ApplicationRoleClaims");
            Delete.Table("ApplicationRoles");
            Delete.Table("ApplicationUsers");
        }
    }
}
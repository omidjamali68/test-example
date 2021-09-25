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

            #region ApplicationIdentities

            var script = _sourceManager.Read("UserManagement_Initial.sql");
            Execute.Sql(script);

            #endregion

            #region RoutinePayments

            Create.Table("RoutinePayments")
                .WithColumn("Id").AsInt64().Identity().PrimaryKey()
                .WithColumn("PayVal").AsDouble().NotNullable()
                .WithColumn("PayDate").AsDateTime().NotNullable()
                .WithColumn("IsCharity").AsBoolean().WithDefaultValue(false)
                .WithColumn("ApplicationUserId").AsGuid().NotNullable()
                .ForeignKey("FK_ApplicationUsers_RoutinePayments", "ApplicationUsers", "Id")
                .OnDelete(Rule.Cascade);

            #endregion

            #region PaymentImages

            Create.Table("PaymentImages")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("FileAddress").AsString(400).NotNullable()
                .WithColumn("RoutinePaymentId").AsInt64().NotNullable()
                .ForeignKey("FK_RoutinePayments_PaymentImages", "RoutinePayments", "Id")
                .OnDelete(Rule.Cascade);

            #endregion

            #region Managements

            Create.Table("Managements")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("DateIn").AsDateTime().NotNullable()
                .WithColumn("DateOut").AsDateTime().Nullable()
                .WithColumn("ApplicationUserId").AsGuid().NotNullable()
                .ForeignKey("FK_ApplicationUsers_Managements", "ApplicationUsers", "Id")
                .OnDelete(Rule.Cascade);

            #endregion

            #region Loans

            Create.Table("Loans")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("LoanVal").AsDouble().NotNullable()
                .WithColumn("InstallmentCount").AsInt16().NotNullable()
                .WithColumn("InstallmentMonthPeriod").AsInt16().NotNullable()
                .WithColumn("InstallmentVal").AsDouble().NotNullable()
                .WithColumn("GetDate").AsDateTime().NotNullable()
                .WithColumn("IsFinished").AsBoolean().WithDefaultValue(false)
                .WithColumn("ApplicationUserId").AsGuid().NotNullable()
                .ForeignKey("FK_ApplicationUsers_Loans", "ApplicationUsers", "Id")
                .OnDelete(Rule.Cascade);

            #endregion

            #region LoanInstallments

            Create.Table("LoanInstallments")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("InstallmentDate").AsDateTime().NotNullable()
                .WithColumn("PayVal").AsDouble().NotNullable()
                .WithColumn("PayDate").AsDateTime().Nullable()
                .WithColumn("LoanId").AsInt32().NotNullable()
                .ForeignKey("FK_Loans_LoanInstallments", "Loans", "Id")
                .OnDelete(Rule.Cascade);

            #endregion

        }

        public override void Down()
        {
            #region LoanInstallments

            Delete.Table("LoanInstallments");

            #endregion

            #region Loans

            Delete.Table("Loans");

            #endregion

            #region Managements

            Delete.Table("Managements");

            #endregion

            #region RoutinePaymentDocuments

            Delete.Table("RoutinePaymentDocuments");

            #endregion

            #region RoutinePayments

            Delete.Table("RoutinePayments");

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

        }
    }
}
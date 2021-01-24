using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class empAppraisalMgrCommentUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmployeeSignOff",
                table: "EmployeeAppraisals",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsManagerSignOff",
                table: "EmployeeAppraisals",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ManagerComment",
                table: "EmployeeAppraisals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmployeeSignOff",
                table: "EmployeeAppraisals");

            migrationBuilder.DropColumn(
                name: "IsManagerSignOff",
                table: "EmployeeAppraisals");

            migrationBuilder.DropColumn(
                name: "ManagerComment",
                table: "EmployeeAppraisals");
        }
    }
}

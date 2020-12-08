using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class depaartmentExitApprovalUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanClearEmployeeOnExit",
                table: "Departments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanClearEmployeeOnExit",
                table: "Departments");
        }
    }
}

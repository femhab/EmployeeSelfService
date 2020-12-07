using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class approvalCofigUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Processor",
                table: "EmployeesApprovalConfig",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Processor",
                table: "EmployeesApprovalConfig");
        }
    }
}

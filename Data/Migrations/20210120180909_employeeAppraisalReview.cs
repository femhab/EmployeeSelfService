using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class employeeAppraisalReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmployeeReview",
                table: "ApprovalBoards",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeReview",
                table: "ApprovalBoards");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class EXTRAcOMMENT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppraisalTarget",
                table: "EmployeeAppraisals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppraiseeComment",
                table: "EmployeeAppraisals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AreaOfImprovement",
                table: "EmployeeAppraisals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppraisalTarget",
                table: "EmployeeAppraisals");

            migrationBuilder.DropColumn(
                name: "AppraiseeComment",
                table: "EmployeeAppraisals");

            migrationBuilder.DropColumn(
                name: "AreaOfImprovement",
                table: "EmployeeAppraisals");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class netscore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalNetScore",
                table: "EmployeeAppraisals",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalNetScore",
                table: "EmployeeAppraisals");
        }
    }
}

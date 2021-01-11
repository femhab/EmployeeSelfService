using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class traainingCalenderUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attended",
                table: "TrainingCalender");

            migrationBuilder.DropColumn(
                name: "Emp_no",
                table: "TrainingCalender");

            migrationBuilder.DropColumn(
                name: "TrainingCategory",
                table: "TrainingCalender");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Attended",
                table: "TrainingCalender",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Emp_no",
                table: "TrainingCalender",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingCategory",
                table: "TrainingCalender",
                nullable: true);
        }
    }
}

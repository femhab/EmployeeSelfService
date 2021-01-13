using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class updatedEmpAppraisal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Counselling",
                table: "EmployeeAppraisals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Development",
                table: "EmployeeAppraisals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisciplinaryAction",
                table: "EmployeeAppraisals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherDetail",
                table: "EmployeeAppraisals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Promotion",
                table: "EmployeeAppraisals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Redeployment",
                table: "EmployeeAppraisals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Strenght",
                table: "EmployeeAppraisals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Training",
                table: "EmployeeAppraisals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Weekness",
                table: "EmployeeAppraisals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Counselling",
                table: "EmployeeAppraisals");

            migrationBuilder.DropColumn(
                name: "Development",
                table: "EmployeeAppraisals");

            migrationBuilder.DropColumn(
                name: "DisciplinaryAction",
                table: "EmployeeAppraisals");

            migrationBuilder.DropColumn(
                name: "OtherDetail",
                table: "EmployeeAppraisals");

            migrationBuilder.DropColumn(
                name: "Promotion",
                table: "EmployeeAppraisals");

            migrationBuilder.DropColumn(
                name: "Redeployment",
                table: "EmployeeAppraisals");

            migrationBuilder.DropColumn(
                name: "Strenght",
                table: "EmployeeAppraisals");

            migrationBuilder.DropColumn(
                name: "Training",
                table: "EmployeeAppraisals");

            migrationBuilder.DropColumn(
                name: "Weekness",
                table: "EmployeeAppraisals");
        }
    }
}

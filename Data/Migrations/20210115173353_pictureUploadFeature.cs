using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class pictureUploadFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "Trainings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "EmployeeNOKDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "EmployeeFamilyDependents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "EmployeeNOKDetails");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "EmployeeFamilyDependents");
        }
    }
}

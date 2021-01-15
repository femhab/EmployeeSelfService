using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class trainingUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AmtPerHead",
                table: "Trainings",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "HoursPerDay",
                table: "Trainings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organizer",
                table: "Trainings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Venue",
                table: "Trainings",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AmtPerHead",
                table: "TrainingCalender",
                nullable: true,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmtPerHead",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "HoursPerDay",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "Organizer",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "Venue",
                table: "Trainings");

            migrationBuilder.AlterColumn<decimal>(
                name: "AmtPerHead",
                table: "TrainingCalender",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class traainingNominationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherDetails",
                table: "Trainings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApplied",
                table: "TrainingNomination",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherDetails",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "IsApplied",
                table: "TrainingNomination");
        }
    }
}

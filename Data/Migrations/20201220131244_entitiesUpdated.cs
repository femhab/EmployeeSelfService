using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class entitiesUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RemainingDays",
                table: "Leaves",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingDays",
                table: "Leaves");
        }
    }
}

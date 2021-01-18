using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class lineManagerUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LineManager",
                table: "ContractObjectives",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineManager",
                table: "ContractObjectives");
        }
    }
}

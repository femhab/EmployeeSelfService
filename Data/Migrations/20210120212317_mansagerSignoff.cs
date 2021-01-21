using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class mansagerSignoff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ManagerSignOff",
                table: "ApprovalBoards",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerSignOff",
                table: "ApprovalBoards");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class appraisalSignOff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SignOff",
                table: "ApprovalBoards",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignOff",
                table: "ApprovalBoards");
        }
    }
}

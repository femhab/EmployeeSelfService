using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class contractComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "ContractObjectives",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "ContractObjectives");
        }
    }
}

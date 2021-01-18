using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class signedOffonPIP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSignedOff",
                table: "PIP",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSignedOff",
                table: "PIP");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class orderNumberOnCategoryItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "AppraisalCategoryItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "AppraisalCategoryItems");
        }
    }
}

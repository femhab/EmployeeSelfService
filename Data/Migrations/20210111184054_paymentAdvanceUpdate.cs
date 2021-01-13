using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class paymentAdvanceUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastProcessor",
                table: "PaymentAdvances",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastProcessor",
                table: "PaymentAdvances");
        }
    }
}

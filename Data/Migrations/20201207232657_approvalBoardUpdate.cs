using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class approvalBoardUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalBoards_Employees_ApprovalProcessorId",
                table: "ApprovalBoards");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalBoards_ApprovalProcessorId",
                table: "ApprovalBoards");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalProcessor",
                table: "ApprovalBoards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalProcessor",
                table: "ApprovalBoards");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalBoards_ApprovalProcessorId",
                table: "ApprovalBoards",
                column: "ApprovalProcessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalBoards_Employees_ApprovalProcessorId",
                table: "ApprovalBoards",
                column: "ApprovalProcessorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

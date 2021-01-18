using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class pipUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PIP_Employees_EmployeeId",
                table: "PIP");

            migrationBuilder.DropForeignKey(
                name: "FK_PIPItems_PIP_PIPId",
                table: "PIPItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PIPItems",
                table: "PIPItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PIP",
                table: "PIP");

            migrationBuilder.RenameTable(
                name: "PIPItems",
                newName: "PaymentAdvanceTracks");

            migrationBuilder.RenameTable(
                name: "PIP",
                newName: "PIPs");

            migrationBuilder.RenameIndex(
                name: "IX_PIPItems_PIPId",
                table: "PaymentAdvanceTracks",
                newName: "IX_PaymentAdvanceTracks_PIPId");

            migrationBuilder.RenameIndex(
                name: "IX_PIP_EmployeeId",
                table: "PIPs",
                newName: "IX_PIPs_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentAdvanceTracks",
                table: "PaymentAdvanceTracks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PIPs",
                table: "PIPs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentAdvanceTracks_PIPs_PIPId",
                table: "PaymentAdvanceTracks",
                column: "PIPId",
                principalTable: "PIPs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PIPs_Employees_EmployeeId",
                table: "PIPs",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentAdvanceTracks_PIPs_PIPId",
                table: "PaymentAdvanceTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_PIPs_Employees_EmployeeId",
                table: "PIPs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PIPs",
                table: "PIPs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentAdvanceTracks",
                table: "PaymentAdvanceTracks");

            migrationBuilder.RenameTable(
                name: "PIPs",
                newName: "PIP");

            migrationBuilder.RenameTable(
                name: "PaymentAdvanceTracks",
                newName: "PIPItems");

            migrationBuilder.RenameIndex(
                name: "IX_PIPs_EmployeeId",
                table: "PIP",
                newName: "IX_PIP_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentAdvanceTracks_PIPId",
                table: "PIPItems",
                newName: "IX_PIPItems_PIPId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PIP",
                table: "PIP",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PIPItems",
                table: "PIPItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PIP_Employees_EmployeeId",
                table: "PIP",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PIPItems_PIP_PIPId",
                table: "PIPItems",
                column: "PIPId",
                principalTable: "PIP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

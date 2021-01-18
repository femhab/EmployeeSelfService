using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class pipUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "PaymentAdvanceTracks");

            migrationBuilder.RenameTable(
                name: "PIPs",
                newName: "PIP");

            migrationBuilder.RenameColumn(
                name: "PublishBy",
                table: "PaymentAdvanceTracks",
                newName: "Emp_No");

            migrationBuilder.RenameColumn(
                name: "PIPId",
                table: "PaymentAdvanceTracks",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentAdvanceTracks_PIPId",
                table: "PaymentAdvanceTracks",
                newName: "IX_PaymentAdvanceTracks_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_PIPs_EmployeeId",
                table: "PIP",
                newName: "IX_PIP_EmployeeId");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "PaymentAdvanceTracks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "PaymentAdvanceTracks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PIP",
                table: "PIP",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PIPItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PIPId = table.Column<Guid>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    PublishBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PIPItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PIPItems_PIP_PIPId",
                        column: x => x.PIPId,
                        principalTable: "PIP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PIPItems_PIPId",
                table: "PIPItems",
                column: "PIPId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentAdvanceTracks_Employees_EmployeeId",
                table: "PaymentAdvanceTracks",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PIP_Employees_EmployeeId",
                table: "PIP",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentAdvanceTracks_Employees_EmployeeId",
                table: "PaymentAdvanceTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_PIP_Employees_EmployeeId",
                table: "PIP");

            migrationBuilder.DropTable(
                name: "PIPItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PIP",
                table: "PIP");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "PaymentAdvanceTracks");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "PaymentAdvanceTracks");

            migrationBuilder.RenameTable(
                name: "PIP",
                newName: "PIPs");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "PaymentAdvanceTracks",
                newName: "PIPId");

            migrationBuilder.RenameColumn(
                name: "Emp_No",
                table: "PaymentAdvanceTracks",
                newName: "PublishBy");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentAdvanceTracks_EmployeeId",
                table: "PaymentAdvanceTracks",
                newName: "IX_PaymentAdvanceTracks_PIPId");

            migrationBuilder.RenameIndex(
                name: "IX_PIP_EmployeeId",
                table: "PIPs",
                newName: "IX_PIPs_EmployeeId");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "PaymentAdvanceTracks",
                nullable: true);

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
    }
}

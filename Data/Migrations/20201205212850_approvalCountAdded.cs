using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class approvalCountAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ProcessorIId",
                table: "EmployeesApprovalConfig",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateTable(
                name: "EmployeeApprovalCounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    Emp_No = table.Column<string>(nullable: true),
                    ApprovalWorkItemId = table.Column<Guid>(nullable: false),
                    MaximumCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeApprovalCounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeApprovalCounts_ApprovalWorkItems_ApprovalWorkItemId",
                        column: x => x.ApprovalWorkItemId,
                        principalTable: "ApprovalWorkItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeApprovalCounts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeApprovalCounts_ApprovalWorkItemId",
                table: "EmployeeApprovalCounts",
                column: "ApprovalWorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeApprovalCounts_EmployeeId",
                table: "EmployeeApprovalCounts",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeApprovalCounts");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProcessorIId",
                table: "EmployeesApprovalConfig",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}

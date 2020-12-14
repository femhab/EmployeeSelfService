using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class activeLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApprovalBoardActiveLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalWorkItemId = table.Column<Guid>(nullable: false),
                    ServiceId = table.Column<Guid>(nullable: false),
                    ActiveLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalBoardActiveLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalBoardActiveLevels_ApprovalWorkItems_ApprovalWorkItemId",
                        column: x => x.ApprovalWorkItemId,
                        principalTable: "ApprovalWorkItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalItems_EmployeeAppraisalId",
                table: "AppraisalItems",
                column: "EmployeeAppraisalId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalBoardActiveLevels_ApprovalWorkItemId",
                table: "ApprovalBoardActiveLevels",
                column: "ApprovalWorkItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppraisalItems_EmployeeAppraisals_EmployeeAppraisalId",
                table: "AppraisalItems",
                column: "EmployeeAppraisalId",
                principalTable: "EmployeeAppraisals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppraisalItems_EmployeeAppraisals_EmployeeAppraisalId",
                table: "AppraisalItems");

            migrationBuilder.DropTable(
                name: "ApprovalBoardActiveLevels");

            migrationBuilder.DropIndex(
                name: "IX_AppraisalItems_EmployeeAppraisalId",
                table: "AppraisalItems");
        }
    }
}

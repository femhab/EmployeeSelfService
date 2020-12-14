using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class recallLeaveUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoOfDays",
                table: "LeaveRecalls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RecallDate",
                table: "LeaveRecalls",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ResumptionDate",
                table: "LeaveRecalls",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "AppraisalItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EmployeeAppraisalId = table.Column<Guid>(nullable: false),
                    AppraisalCategoryId = table.Column<Guid>(nullable: false),
                    AppraisalCategoryItemId = table.Column<Guid>(nullable: false),
                    AppraisalRatingId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppraisalItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppraisalItems_AppraisalCategories_AppraisalCategoryId",
                        column: x => x.AppraisalCategoryId,
                        principalTable: "AppraisalCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppraisalItems_AppraisalCategoryItems_AppraisalCategoryItemId",
                        column: x => x.AppraisalCategoryItemId,
                        principalTable: "AppraisalCategoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppraisalItems_AppraisalRatings_AppraisalRatingId",
                        column: x => x.AppraisalRatingId,
                        principalTable: "AppraisalRatings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAppraisals",
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
                    AppraisalPeriodId = table.Column<Guid>(nullable: false),
                    LastRatingManagerId = table.Column<string>(nullable: true),
                    LastRatingManagerName = table.Column<string>(nullable: true),
                    NextRatingManagerId = table.Column<string>(nullable: true),
                    NextRatingManagerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAppraisals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAppraisals_AppraisalPeriods_AppraisalPeriodId",
                        column: x => x.AppraisalPeriodId,
                        principalTable: "AppraisalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeAppraisals_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalItems_AppraisalCategoryId",
                table: "AppraisalItems",
                column: "AppraisalCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalItems_AppraisalCategoryItemId",
                table: "AppraisalItems",
                column: "AppraisalCategoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalItems_AppraisalRatingId",
                table: "AppraisalItems",
                column: "AppraisalRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAppraisals_AppraisalPeriodId",
                table: "EmployeeAppraisals",
                column: "AppraisalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAppraisals_EmployeeId",
                table: "EmployeeAppraisals",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppraisalItems");

            migrationBuilder.DropTable(
                name: "EmployeeAppraisals");

            migrationBuilder.DropColumn(
                name: "NoOfDays",
                table: "LeaveRecalls");

            migrationBuilder.DropColumn(
                name: "RecallDate",
                table: "LeaveRecalls");

            migrationBuilder.DropColumn(
                name: "ResumptionDate",
                table: "LeaveRecalls");
        }
    }
}
